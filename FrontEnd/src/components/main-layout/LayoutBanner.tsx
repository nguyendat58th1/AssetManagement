import { Menu } from "antd";
import { LogoutOutlined } from "@ant-design/icons";
import { useEffect, useState } from "react";

import { Modal, Button } from "react-bootstrap";
import { ErrorMessage, Field, Form, Formik } from "formik";
import * as Yup from "yup";
import { authenticationService } from "../../services/authentication.service";

import "./modal.css";
import { history } from "../../helpers/history";
import { userService } from "../../services/user.service";

const { SubMenu } = Menu;

function LayoutBanner() {
  const [show, setShow] = useState(false);
  const [showModal, setshowModal] = useState(false);
  const [passwordChanged, setpasswordChanged] = useState(false);
  const handleShowModal = () => setshowModal(true);
  const handleCloseModal = () => setshowModal(false);
  const handlePasswordChange = () => {
    setpasswordChanged(true);
  };

  useEffect(() => {
    if (authenticationService.currentUserValue.onFirstLogin === 1) {
      setShow(true);
    }
  });

  let OnLogout = () => {
    sessionStorage.clear();
    authenticationService.logout();
    history.push("/login");
    window.location.href = "/login";
  };

  return (
    <>
      {sessionStorage.getItem("username") && (
        <Menu
          key="1"
          style={{
            backgroundColor: "#cf2338",
            width: "100%",
            zIndex: 1,
            position: "fixed",
          }}
          theme="light"
          mode="horizontal"
          defaultSelectedKeys={["2"]}
        >
          <SubMenu
            title="Asset Management Portal"
            style={{ fontFamily: "Roboto", color: "white", fontSize: 20 }}
          ></SubMenu>
          <SubMenu
            title={sessionStorage.getItem("username")}
            style={{
              float: "right",
              fontFamily: "Roboto",
              color: "white",
              fontSize: 20,
            }}
          >
            <Menu.Item key="setting:1">
              <span onClick={handleShowModal}>Change Password</span>
            </Menu.Item>
            <Menu.Item key="setting:2">
              <span onClick={OnLogout}>
                <LogoutOutlined />
                Logout
              </span>
            </Menu.Item>
          </SubMenu>
        </Menu>
      )}
      {!sessionStorage.getItem("username") && (
        <Menu
          key="1"
          style={{
            backgroundColor: "#cf2338",
            width: "100%",
            zIndex: 1,
            position: "fixed",
          }}
          theme="light"
          mode="horizontal"
          defaultSelectedKeys={["2"]}
        >
          <SubMenu
            title="Asset Management Portal"
            style={{ fontFamily: "Roboto", color: "white", fontSize: 20 }}
          ></SubMenu>
        </Menu>
      )}
      {/* Modal */}
      {authenticationService.currentUserValue.id && (
        <Modal show={show} onHide={OnLogout} backdrop="static" keyboard={false}>
          <Modal.Header>
            <Modal.Title>Change Password</Modal.Title>
          </Modal.Header>
          {!passwordChanged && (
            <Formik
              initialValues={{
                newpassword: "",
              }}
              validationSchema={Yup.object().shape({
                newpassword: Yup.string().required("Password is required"),
              })}
              onSubmit={({ newpassword }, { setStatus, setSubmitting }) => {
                setStatus();
                userService
                  .changePassword(
                    authenticationService.currentUserValue.id,
                    "string",
                    newpassword
                  )
                  .then(
                    () => {
                      setSubmitting(false);

                      handlePasswordChange();
                    },
                    (error) => {
                      setSubmitting(false);
                      setStatus(error);
                    }
                  );
              }}
              render={({ errors, status, touched, isSubmitting }) => (
                <Form>
                  <Modal.Body>
                    <p>This is the first time you logged in.</p>
                    <p>You have to change your password to continue.</p>
                    <div className="form-group row">
                      <div className="col-4">Password</div>
                      <div className="col-8">
                        <Field
                          name="newpassword"
                          type="password"
                          className={
                            "form-control" +
                            (errors.newpassword && touched.newpassword
                              ? " is-invalid"
                              : "")
                          }
                        />
                      </div>
                      <ErrorMessage
                        name="newpassword"
                        component="div"
                        className="invalid-feedback"
                      />
                    </div>
                  </Modal.Body>
                  <Modal.Footer>
                    <div className="form-group">
                      <button
                        type="submit"
                        className="btn btn-primary"
                        disabled={isSubmitting}
                      >
                        Save
                      </button>
                      {isSubmitting && (
                        <img src="data:image/gif;base64,R0lGODlhEAAQAPIAAP///wAAAMLCwkJCQgAAAGJiYoKCgpKSkiH/C05FVFNDQVBFMi4wAwEAAAAh/hpDcmVhdGVkIHdpdGggYWpheGxvYWQuaW5mbwAh+QQJCgAAACwAAAAAEAAQAAADMwi63P4wyklrE2MIOggZnAdOmGYJRbExwroUmcG2LmDEwnHQLVsYOd2mBzkYDAdKa+dIAAAh+QQJCgAAACwAAAAAEAAQAAADNAi63P5OjCEgG4QMu7DmikRxQlFUYDEZIGBMRVsaqHwctXXf7WEYB4Ag1xjihkMZsiUkKhIAIfkECQoAAAAsAAAAABAAEAAAAzYIujIjK8pByJDMlFYvBoVjHA70GU7xSUJhmKtwHPAKzLO9HMaoKwJZ7Rf8AYPDDzKpZBqfvwQAIfkECQoAAAAsAAAAABAAEAAAAzMIumIlK8oyhpHsnFZfhYumCYUhDAQxRIdhHBGqRoKw0R8DYlJd8z0fMDgsGo/IpHI5TAAAIfkECQoAAAAsAAAAABAAEAAAAzIIunInK0rnZBTwGPNMgQwmdsNgXGJUlIWEuR5oWUIpz8pAEAMe6TwfwyYsGo/IpFKSAAAh+QQJCgAAACwAAAAAEAAQAAADMwi6IMKQORfjdOe82p4wGccc4CEuQradylesojEMBgsUc2G7sDX3lQGBMLAJibufbSlKAAAh+QQJCgAAACwAAAAAEAAQAAADMgi63P7wCRHZnFVdmgHu2nFwlWCI3WGc3TSWhUFGxTAUkGCbtgENBMJAEJsxgMLWzpEAACH5BAkKAAAALAAAAAAQABAAAAMyCLrc/jDKSatlQtScKdceCAjDII7HcQ4EMTCpyrCuUBjCYRgHVtqlAiB1YhiCnlsRkAAAOwAAAAAAAAAAAA==" />
                      )}
                    </div>
                    {status && (
                      <div className={"alert alert-danger"}>{status}</div>
                    )}
                    <Button variant="secondary" onClick={OnLogout}>
                      Close and Logout
                    </Button>
                  </Modal.Footer>
                </Form>
              )}
            />
          )}
          {passwordChanged && (
            <div>
              <Modal.Body>
                <p>Your Password has been changed successfully!</p>
              </Modal.Body>
              <Modal.Footer>
                <Button variant="secondary" onClick={OnLogout}>
                  Close and Logout
                </Button>
              </Modal.Footer>
            </div>
          )}
        </Modal>
      )}
      {authenticationService.currentUserValue.id && (
        <Modal
          show={showModal}
          onHide={OnLogout}
          backdrop="static"
          keyboard={false}
        >
          <Modal.Header>
            <Modal.Title>Change Password</Modal.Title>
          </Modal.Header>
          {!passwordChanged && (
            <Formik
              initialValues={{
                oldpassword: "",
                newpassword: "",
              }}
              validationSchema={Yup.object().shape({
                oldpassword: Yup.string().required("Password is required"),
                newpassword: Yup.string().required("Password is required"),
              })}
              onSubmit={(
                { oldpassword, newpassword },
                { setStatus, setSubmitting }
              ) => {
                setStatus();
                let id = authenticationService.currentUserValue.id;
                userService.changePassword(id, oldpassword, newpassword).then(
                  () => {
                    setSubmitting(false);
                    handlePasswordChange();
                  },
                  (error) => {
                    setSubmitting(false);
                    setStatus(error);
                  }
                );
              }}
              render={({ errors, status, touched, isSubmitting }) => (
                <Form>
                  <Modal.Body>
                    <div className="form-group row m-3">
                      <div className="col-4">Old Password</div>
                      <div className="col-8">
                        <Field
                          name="oldpassword"
                          type="password"
                          className={
                            "form-control" +
                            (errors.oldpassword && touched.oldpassword
                              ? " is-invalid"
                              : "")
                          }
                        />
                        {status && (
                          <div
                            className={"mt-2"}
                            style={{
                              backgroundColor: "#ffe6e6",
                              color: "red",
                              borderRadius: "4px",
                            }}
                          >
                            {status}
                          </div>
                        )}
                      </div>
                      <ErrorMessage
                        name="newpassword"
                        component="div"
                        className="invalid-feedback"
                      />
                    </div>
                    <div className="form-group row m-3">
                      <div className="col-4">New Password</div>
                      <div className="col-8">
                        <Field
                          name="newpassword"
                          type="password"
                          className={
                            "form-control" +
                            (errors.newpassword && touched.newpassword
                              ? " is-invalid"
                              : "")
                          }
                        />
                      </div>
                      <ErrorMessage
                        name="newpassword"
                        component="div"
                        className="invalid-feedback"
                      />
                    </div>
                  </Modal.Body>
                  <Modal.Footer>
                    <div className="form-group">
                      <button
                        type="submit"
                        className="btn btn-primary"
                        disabled={isSubmitting}
                      >
                        Save
                      </button>
                      {isSubmitting && (
                        <img src="data:image/gif;base64,R0lGODlhEAAQAPIAAP///wAAAMLCwkJCQgAAAGJiYoKCgpKSkiH/C05FVFNDQVBFMi4wAwEAAAAh/hpDcmVhdGVkIHdpdGggYWpheGxvYWQuaW5mbwAh+QQJCgAAACwAAAAAEAAQAAADMwi63P4wyklrE2MIOggZnAdOmGYJRbExwroUmcG2LmDEwnHQLVsYOd2mBzkYDAdKa+dIAAAh+QQJCgAAACwAAAAAEAAQAAADNAi63P5OjCEgG4QMu7DmikRxQlFUYDEZIGBMRVsaqHwctXXf7WEYB4Ag1xjihkMZsiUkKhIAIfkECQoAAAAsAAAAABAAEAAAAzYIujIjK8pByJDMlFYvBoVjHA70GU7xSUJhmKtwHPAKzLO9HMaoKwJZ7Rf8AYPDDzKpZBqfvwQAIfkECQoAAAAsAAAAABAAEAAAAzMIumIlK8oyhpHsnFZfhYumCYUhDAQxRIdhHBGqRoKw0R8DYlJd8z0fMDgsGo/IpHI5TAAAIfkECQoAAAAsAAAAABAAEAAAAzIIunInK0rnZBTwGPNMgQwmdsNgXGJUlIWEuR5oWUIpz8pAEAMe6TwfwyYsGo/IpFKSAAAh+QQJCgAAACwAAAAAEAAQAAADMwi6IMKQORfjdOe82p4wGccc4CEuQradylesojEMBgsUc2G7sDX3lQGBMLAJibufbSlKAAAh+QQJCgAAACwAAAAAEAAQAAADMgi63P7wCRHZnFVdmgHu2nFwlWCI3WGc3TSWhUFGxTAUkGCbtgENBMJAEJsxgMLWzpEAACH5BAkKAAAALAAAAAAQABAAAAMyCLrc/jDKSatlQtScKdceCAjDII7HcQ4EMTCpyrCuUBjCYRgHVtqlAiB1YhiCnlsRkAAAOwAAAAAAAAAAAA==" />
                      )}
                    </div>
                    <Button variant="secondary" onClick={handleCloseModal}>
                      Cancel
                    </Button>
                  </Modal.Footer>
                </Form>
              )}
            />
          )}
          {passwordChanged && (
            <div>
              <Modal.Body>
                <p>Your Password has been changed successfully!</p>
              </Modal.Body>
              <Modal.Footer>
                <Button variant="secondary" onClick={handleCloseModal}>
                  Close
                </Button>
              </Modal.Footer>
            </div>
          )}
        </Modal>
      )}
    </>
  );
}

export default LayoutBanner;

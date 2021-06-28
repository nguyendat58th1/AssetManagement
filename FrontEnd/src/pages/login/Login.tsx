import { Button, Form, Input } from "antd";
import Title from "antd/lib/typography/Title";
import React from "react";
import { useState } from "react";
import { useHistory } from "react-router-dom";
import { UserLogin, UserType } from "../../models/UserModel";
import { authenticationService } from "../../services/authentication.service";
import { AuthenticationService } from "../../services/AuthenticationService";

export function Login() {
  const layout = {
    labelCol: {
      span: 16,
      offset: 3,
      pull: 9,
    },
    wrapperCol: {
      span: 16,
      pull: 9,
    },
  };
  const tailLayout = {
    wrapperCol: {},
  };

  let history = useHistory();
  const service = AuthenticationService.getInstance();

  const onFinish = (data: UserLogin) => {
    setSubmitting(true);
    authenticationService.login(data.username, data.password).then(
      (user) => {
        (async () => {
          let userLogin = await service.login(data);
          sessionStorage.setItem("id", userLogin.id.toString());
          sessionStorage.setItem("type", UserType[userLogin.type]);
          sessionStorage.setItem("username", userLogin.userName);
          sessionStorage.setItem("token", userLogin.token);
          sessionStorage.setItem("location", userLogin.location.toString());
          history.replace("/home");
        })();
      },
      (error) => {
        setSubmitting(false);
        setStatus(error);
      }
    );
  };

  const onFinishFailed = (errorInfo: any) => {
    console.log("Failed:", errorInfo);
  };

  //

  const [isSubmitting, setSubmitting] = useState(false);
  const [status, setStatus] = useState(null);
  return (
    <>
      <Title style={{ color: "rgb(233, 66, 77)", marginTop: "1em" }}>
        Login
      </Title>
      <Form
        {...layout}
        name="basic"
        onFinish={onFinish}
        onFinishFailed={onFinishFailed}
      >
        <Form.Item
          label="Username"
          name="username"
          rules={[
            {
              required: true,
              message: "Please input your username!",
            },
          ]}
        >
          <Input />
        </Form.Item>

        <Form.Item
          label="Password"
          name="password"
          rules={[
            {
              required: true,
              message: "Please input your password!",
            },
          ]}
        >
          <Input.Password />
        </Form.Item>

        <Form.Item {...tailLayout}>
          <Button
            style={{
              backgroundColor: "#e9424d",
              color: "white",
            }}
            htmlType="submit"
            disabled={isSubmitting}
          >
            Login
          </Button>
          {isSubmitting && (
            <img
              alt=""
              style={{ marginLeft: "1em", width: "1.5em" }}
              src="data:image/gif;base64,R0lGODlhEAAQAPIAAP///wAAAMLCwkJCQgAAAGJiYoKCgpKSkiH/C05FVFNDQVBFMi4wAwEAAAAh/hpDcmVhdGVkIHdpdGggYWpheGxvYWQuaW5mbwAh+QQJCgAAACwAAAAAEAAQAAADMwi63P4wyklrE2MIOggZnAdOmGYJRbExwroUmcG2LmDEwnHQLVsYOd2mBzkYDAdKa+dIAAAh+QQJCgAAACwAAAAAEAAQAAADNAi63P5OjCEgG4QMu7DmikRxQlFUYDEZIGBMRVsaqHwctXXf7WEYB4Ag1xjihkMZsiUkKhIAIfkECQoAAAAsAAAAABAAEAAAAzYIujIjK8pByJDMlFYvBoVjHA70GU7xSUJhmKtwHPAKzLO9HMaoKwJZ7Rf8AYPDDzKpZBqfvwQAIfkECQoAAAAsAAAAABAAEAAAAzMIumIlK8oyhpHsnFZfhYumCYUhDAQxRIdhHBGqRoKw0R8DYlJd8z0fMDgsGo/IpHI5TAAAIfkECQoAAAAsAAAAABAAEAAAAzIIunInK0rnZBTwGPNMgQwmdsNgXGJUlIWEuR5oWUIpz8pAEAMe6TwfwyYsGo/IpFKSAAAh+QQJCgAAACwAAAAAEAAQAAADMwi6IMKQORfjdOe82p4wGccc4CEuQradylesojEMBgsUc2G7sDX3lQGBMLAJibufbSlKAAAh+QQJCgAAACwAAAAAEAAQAAADMgi63P7wCRHZnFVdmgHu2nFwlWCI3WGc3TSWhUFGxTAUkGCbtgENBMJAEJsxgMLWzpEAACH5BAkKAAAALAAAAAAQABAAAAMyCLrc/jDKSatlQtScKdceCAjDII7HcQ4EMTCpyrCuUBjCYRgHVtqlAiB1YhiCnlsRkAAAOwAAAAAAAAAAAA=="
            />
          )}
        </Form.Item>

        {status && <div className={"alert alert-danger"}>{status}</div>}
      </Form>
    </>
  );
}

import {
  Button,
  Col,
  DatePicker,
  Form,
  Input,
  message,
  Modal,
  Row,
  Space,
  Table,
} from "antd";
import React, { useEffect, useState } from "react";
import { Link, useHistory } from "react-router-dom";
import { AssignmentModel } from "../../models/Assignment";
import { User, UserType } from "../../models/UserModel";
import { AssignmentsService } from "../../services/AssignmentService";
import { UserService } from "../../services/UserService";
import { SearchOutlined } from "@ant-design/icons";
import { AssetService } from "../../services/AssetService";
import { Asset } from "../../models/Asset";



const { TextArea } = Input;

export function CreateAssignment() {

  const [form] = Form.useForm();
  const [, forceUpdate] = useState({}); // To disable submit button at the beginning.
  const [isModalUserVisible, setIsModalUserVisible] = useState(false);
  const [isModalAssetVisible, setIsModalAssetVisible] = useState(false);
  const [user, setUser] = useState<User[]>([]);
  const [asset, setAsset] = useState<Asset[]>([]);
  

  let userService = UserService.getInstance();
  let assetService = AssetService.getInstance();
  let service = AssignmentsService.getInstance();
  
  useEffect(() => {
    forceUpdate({});
  }, []);

  useEffect(() => {
    (async () => {
      let listUser = await userService.getAllUsers();
      setUser(listUser);
    })();
  }, []);

  useEffect(() => {
    (async () => {
      let listAsset = await assetService.getAllAssets();
      setAsset(listAsset);
    })();
  }, []);


  let history = useHistory();

  const today = new Date();

  const validateAssignedDate = async (rule: any, value: any, callback: any) => {
    today.setHours(0);
    today.setMinutes(0);
    today.setSeconds(0);
    if (value && value._d <  today) {
      throw new Error("Assign date is earlier than current time");
    }
  };

  const onFinish = (data: AssignmentModel) => {
    (async () => {
      await service.create(data);
      message.success("Created Successfully")
      history.push("/assignments");
    })();
  };
  const dateFormat = "YYYY-MM-DD";

  const onFinishFailed = () => {
    message.error("Create Failed");
  };

  const onSearchUserButtonClicked = (values: any) => {
    (async () => {
      let { searchText } = values;
      let listUser: any;
      if (searchText.length === 0) {
        listUser = await userService.getAllUsers();
      } else {
        listUser = await userService.getUsersBySearch(searchText);
      }
      setUser(listUser);
    })();
  };

  const onSearchAssetButtonClicked = (values: any) => {
    (async () => {
      let { searchText } = values;
      let listAsset: any;
      if (searchText.length === 0) {
        listAsset = await assetService.getAllAssets();
      } else {
        listAsset = await assetService.getAssetsBySearch(searchText);
      }
      setAsset(listAsset);
    })();
  };

  const columnsUser: any = [
    {
      title: "Staff code",
      dataIndex: "staffCode",
      key: "staffCode",
      sorter: (a: User, b: User) => a.staffCode.localeCompare(b.staffCode),
      sortDirections: ["ascend", "descend"],
    },
    {
      title: "Full name",
      dataIndex: "fullName",
      key: "fullName",
      sorter: (a: User, b: User) => {
        let fullNameA = `${a.firstName} ${a.lastName}`;
        let fullNameB = `${b.firstName} ${b.lastName}`;
        return fullNameA.localeCompare(fullNameB);
      },
      render: (text: any, record: User, index: number) => {
        return <div>{`${record.firstName} ${record.lastName}`}</div>;
      },
      sortDirections: ["ascend", "descend"],
    },
    {
      title: "Type",
      dataIndex: "type",
      key: "type",
      render: (text: any, record: User, index: number) => {
        return (
          <>
          {record.type === UserType.Admin && <div>Admin</div>}
          {record.type === UserType.User && <div>User</div>}
          </>
        )
      },
      sorter: (a: User, b: User) => a.type - b.type,
      sortDirections: ["ascend", "descend"],
    },
  ];

  const columnsAsset: any = [
    {
      title: "Asset code",
      dataIndex: "assetCode",
      key: "assetCode",
      sorter: (a: Asset, b: Asset) => a.assetCode.localeCompare(b.assetCode),
      sortDirections: ["ascend", "descend"],
    },
    {
      title: "Asset name",
      dataIndex: "assetName",
      key: "assetName",
      sorter: (a: Asset, b: Asset) => a.assetName.localeCompare(b.assetName),
      sortDirections: ["ascend", "descend"],
    },
    {
      title: "Category",
      dataIndex: "categoryId",
      key: "categoryId",
      sorter: (a: Asset, b: Asset) => a.category.categoryName.localeCompare(b.category.categoryName),
      render: (text: any, record: Asset, index: number) => {
       
        return <div>{ record.category.categoryName}</div>;
      },
      sortDirections: ["ascend", "descend"],
    },
  ];

  const rowSelectionUser = {
    onChange: (selectedRowKeys: React.Key[], selectedRows: User[]) => {
      let name =
        selectedRows.map((p) => p.firstName).toString() +
        " " +
        selectedRows.map((p) => p.lastName).toString();
      let userId = selectedRows.map((p) => p.id).toString();
      form.setFieldsValue({
        fullName: name,
        assignedToUserId: userId,
      });
    },
  };

  const rowSelectionAsset = {
    onChange: (selectedRowKeys: React.Key[], selectedRows: Asset[]) => {
      let name = selectedRows.map((p) => p.assetName).toString();
      let assetId = selectedRows.map((p) => p.id).toString();
      form.setFieldsValue({
        assetName: name,
        assetId: assetId,
      });
    },
  };

  const onSearchUser = () => {
    setIsModalUserVisible(true);
  };

  const handleUserOk = () => {
    setIsModalUserVisible(false);
  };

  const handleUserCancel = () => {
    setIsModalUserVisible(false);
  };

  const onSearchAsset = () => {
    setIsModalAssetVisible(true);
  };

  const handleAssetOk = () => {
    setIsModalAssetVisible(false);
  };

  const handleAssetCancel = () => {
    setIsModalAssetVisible(false);
  };

  return (
    <>
      <Col span={11}>
        <h4>Create New Assignment</h4>
      </Col>
      <Col span={22}>
        <Form
          style= {{marginLeft : 200}}
          name="basic"
          form={form}
          onFinish={onFinish}
          onFinishFailed={onFinishFailed}
        >
          <Form.Item
            label="User"
            name="fullName"
            rules={[
              { required: true, message: "User is required!" },
              { max: 50, message: "Maximum 50 characters!" },
              { whitespace: true, message: "User can not be empty!" },
            ]}
            labelCol={{ span: 4 }} wrapperCol={{ span: 11 }} labelAlign="left"
            hasFeedback
          >
            <Input.Search
              placeholder="click icon to search user"
              onSearch={onSearchUser}
              readOnly
              style={{width: "83%" }}
            />
          </Form.Item>

          <Form.Item
            label="assignedToUserId"
            name="assignedToUserId"
            rules={[{ required: true }]}
            hidden={true}
          >
            <Input readOnly />
          </Form.Item>

          <Form.Item
            label="Asset"
            name="assetName"
            rules={[
              { required: true, message: "Asset is required!" },
              { max: 50, message: "Maximum 50 characters!" },
              { whitespace: true, message: "Asset can not be empty!" },
            ]}
            labelCol={{ span: 4 }} wrapperCol={{ span: 11 }} labelAlign="left"
            hasFeedback
          >
            <Input.Search
              placeholder="click icon to search asset"
              onSearch={onSearchAsset}
              readOnly
              style={{width: "83%" }}
            />
          </Form.Item>
          <Form.Item
            label="assetId"
            name="assetId"
            rules={[{ required: true }]}
            hidden={true}
          >
            <Input readOnly />
          </Form.Item>

          <Form.Item
            hasFeedback
            name="assignedDate"
            label="Assigned Date"
            rules={[
              { required: true, message: "Please select assigned date!" },
              { validator: validateAssignedDate },
            ]}
            labelCol={{ span: 4 }} wrapperCol={{ span: 11 }} labelAlign="left"
          >
            <DatePicker format={dateFormat}  style={{ width : "83%"}} />
          </Form.Item>

          <Form.Item name="note" label="Note" hasFeedback labelCol={{ span: 4 }} wrapperCol={{ span: 11 }} labelAlign="left">
            <TextArea rows={4}  style={{width: "83%" }}/>
          </Form.Item>

          <Form.Item style ={{marginLeft : 24}} shouldUpdate>
            {() => (
              <Space >
                <Button
                  style={{ backgroundColor: "#e9424d", color: "white" }}
                  type="primary"
                  htmlType="submit"
                  disabled={
                    !form.isFieldsTouched(true) ||
                    !!form
                      .getFieldsError()
                      .filter(({ errors }) => errors.length).length
                  }
                >
                  Save
                </Button>
                <Button>
                  <Link to="/assignments">Cancel</Link>
                </Button>
              </Space>
            )}
          </Form.Item>
        </Form>
      </Col>
      <Modal
        title="Select User"
        visible={isModalUserVisible}
        onOk={handleUserOk}
        cancelButtonProps={{ style: { display: 'none' } }}
        onCancel={handleUserCancel}
      >
        <Col span={18} offset={8}>
          <Form
            onFinish={onSearchUserButtonClicked}
            initialValues={{
              searchText: null,
            }}
          >
            <Row style={{marginBottom : 20}}>
              <Col span={18}>
                <Form.Item name="searchText" className="no-margin-no-padding">
                  <Input
                    allowClear
                    style={{ width: "116%" }}
                    placeholder="e.g. Bob/SD0001"
                  />
                </Form.Item>
              </Col>
              <Col offset={3} >
                <Form.Item className="no-margin-no-padding">
                  <Button
                    size="middle"
                    icon={<SearchOutlined />}
                    type="primary"
                    htmlType="submit"
                  />
                </Form.Item>
              </Col>
            </Row>
          </Form>
        </Col>
        <Table
          dataSource={user}
          columns={columnsUser}
          scroll={{ y: 400 }}
          pagination={false}
          rowSelection={{ type: "radio", ...rowSelectionUser }}
          rowKey={(record) => record.id}
        />
      </Modal>

      <Modal
        title="Select Asset"
        visible={isModalAssetVisible}
        onOk={handleAssetOk}
        cancelButtonProps={{ style: { display: 'none' } }}
        onCancel={handleAssetCancel}
      >
        <Col span={18} offset={8} >
          <Form
            onFinish={onSearchAssetButtonClicked}
            initialValues={{
              searchText: null,
            }}
          >
            <Row style={{marginBottom : 20}}>
              <Col span={18}>
                <Form.Item name="searchText" className="no-margin-no-padding">
                  <Input
                    allowClear
                    style={{ width: "116%" }}
                    placeholder="e.g. Personal Computer/SD0001"
                  />
                </Form.Item>
              </Col>
              <Col offset={3}>
                <Form.Item className="no-margin-no-padding">
                  <Button
                    size="middle"
                    icon={<SearchOutlined />}
                    type="primary"
                    htmlType="submit"
                  />
                </Form.Item>
              </Col>
            </Row>
          </Form>
        </Col>
        <Table
          dataSource={asset}
          columns={columnsAsset}
          scroll={{ y: 400 }}
          pagination={false}
          rowSelection={{ type: "radio", ...rowSelectionAsset }}
          rowKey={(record) => record.id}
        />
      </Modal>
    </>
  );
}

import { Button, Col, DatePicker, Form, Input, message, Radio, Select, Space } from "antd";
import React, { useEffect, useState } from "react";
import { Link, useHistory } from "react-router-dom";
import { AssetState, CreateAssetModel } from "../../models/Asset";
import { AssetCategory, CreateCategoryModel } from "../../models/AssetCategory";
import { AssetCategoryService } from "../../services/AssetCategoryService";
import { AssetService } from "../../services/AssetService";
import { CategoryCreateForm } from "../../components/CategoryCreateForm";
import TextArea from "antd/lib/input/TextArea";

const { Option } = Select;

export function CreateAsset() {
    
  const tailLayout = {
    wrapperCol: {
      span: 35,
      offset: 3,
      pull: 1
    },
  };

    const [form] = Form.useForm();
    const [, forceUpdate] = useState({}); // To disable submit button at the beginning.
    const [visible, setVisible] = useState(false);
    const [categories, setCategory] = useState<AssetCategory[]>([]);
    const [update, setUpdate] = useState(false);
    let assetService = AssetService.getInstance();
    let categoryService = AssetCategoryService.getInstance();
    let history = useHistory();
    const location: number = Number(sessionStorage.getItem("location"));

    useEffect(() => {
        (async () => {
            let categories = await categoryService.getAll();
            setCategory(categories);
        })();
    }, [update]);


    const showModal = () => {
        setVisible(true);
    };

    useEffect(() => {
        forceUpdate({});
    }, []);

    const onCreate = (values: CreateCategoryModel) => {

        (async () => {
            await categoryService.create(values);
            setUpdate(pre => !pre);
        })();
        setVisible(false);
    };

    const onAssetFinish = (data: CreateAssetModel) => {
        data.location = location;
        (async () => {
            await assetService.create(data);
            history.push("/assets");
        })();
    };
    const dateFormat = "YYYY-MM-DD";

    const onAssetFinishFailed = () => {
        message.error("Create Failed");
    };

    return (
        <>
            <Col span={11}>
                <h4>Create New Asset</h4>
            </Col>
            <Col span={22}>
                <Form
                    name="basic"
                    form={form}
                    onFinish={onAssetFinish}
                    onFinishFailed={onAssetFinishFailed}
                    style={{ marginLeft: 200 }}
                >
                    <Form.Item
                        label="Name"
                        labelCol={{ span: 4 }}
                        wrapperCol={{ span: 11 }}
                        labelAlign="left"
                        name="assetName"
                        rules={[
                            { required: true, message: "Name is required!" },
                            { max: 100, message: "Maximum 100 characters!" },
                            { whitespace: true, message: "Asset Name can not be empty!" },
                        ]}
                        hasFeedback
                    >
                        <Input style={{ width: "83%" }}  />
                    </Form.Item>
                    <>
                        <Form.Item
                            name="categoryId"
                            labelCol={{ span: 4 }}
                            wrapperCol={{ span: 11 }}
                            labelAlign="left"
                            label="Category"
                            hasFeedback
                            rules={[
                                { required: true, message: "Category is required!" },
                            ]}
                            style={{
                                position: 'relative',
                                textAlign: 'left'
                            }}
                        >
                            <Select style={{ paddingLeft:35, width: "91%" }} >
                                {
                                    categories.map((p) =>
                                    (
                                        <Option key={p.id} value={p.id}>{p.categoryName}</Option>
                                    ))
                                }
                            </Select>
                        </Form.Item>
                        <Button
                            style={{
                                position: 'absolute',
                                right: 301,
                                top: 56
                            }}
                            onClick={showModal}>
                            +
                        </Button>
                    </>
                    <Form.Item
                        label="Specification"
                        labelCol={{ span: 4 }}
                        wrapperCol={{ span: 11 }}
                        labelAlign="left"
                        name="specification"
                        rules={[
                            { required: true, message: "Specification is required!" },
                            { whitespace: true, message: "Specification can not be empty!" },
                        ]}
                        hasFeedback
                    >
                        <TextArea rows={8}  style={{ width: "83%" }} />
                    </Form.Item>

                    <Form.Item
                        hasFeedback
                        name="installedDate"
                        labelCol={{ span: 4 }}
                        wrapperCol={{ span: 11 }}
                        labelAlign="left"
                        label="Installed Date"
                        rules={[{ required: true, message: "Please select installed date!" }]}
                    >
                        <DatePicker format={dateFormat}  style={{ width: "83%" }} />
                    </Form.Item>

                    <Form.Item
                        name="state"
                        labelCol={{ span: 4 }}
                        wrapperCol={{ span: 11 }}
                        labelAlign="left"
                        label="State"
                        hasFeedback
                        rules={[{ required: true, message: "Please select state!" }]}
                    >
                        <Radio.Group>
                            <Radio value={AssetState.AVAILABLE}>Available</Radio>
                            <Radio value={AssetState.NOT_AVAILABLE}>Not Available</Radio>
                        </Radio.Group>
                    </Form.Item>

                    <Form.Item {...tailLayout} shouldUpdate hasFeedback>
                        {() => (
                            <Space>
                                <Button
                                    style={{ backgroundColor: '#e9424d', color: 'white' }}
                                    type="primary"
                                    htmlType="submit"
                                    disabled={
                                        !form.isFieldsTouched(true) ||
                                        !!form.getFieldsError().filter(({ errors }) => errors.length).length
                                    }
                                >
                                    Save
                                </Button>
                                <Button>
                                    <Link to="/assets">
                                        Cancel
                                    </Link>
                                </Button>
                            </Space>
                        )}
                    </Form.Item>
                </Form>
            </Col>
            <CategoryCreateForm
                visible={visible}
                onCreate={onCreate}
                onCancel={() => {
                    setVisible(false);
                }}
            />
        </>
    );
}
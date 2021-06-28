import { Form, Input, Modal } from "antd";
import React, { useState } from "react";
import { AssetCategoryService } from "../services/AssetCategoryService";

type CategoryCreateArgurment = {
    visible: boolean,
    onCreate: any,
    onCancel: any
}

export function CategoryCreateForm({ visible, onCreate, onCancel }: CategoryCreateArgurment) {
    const [form] = Form.useForm();
    let categoryService = AssetCategoryService.getInstance();
    const [didNameExist, setNameExist] = useState(false);
    const [didPrefixExist, setPrefixExist] = useState(false);
    const validateCategoryName = async (rule: any, value: any, callback: any) => {
        (async () => {
            setNameExist(await categoryService.checkNameExist(value));
        })();
        if (didNameExist) {

            throw new Error("Category is already existed. Please enter a different category!");

        }
    };

    const validateCategoryPrefix = async (rule: any, value: any, callback: any) => {
        (async () => {
            setPrefixExist(await categoryService.checkPrefixExist(value));
        })();
        if (didPrefixExist) {

            throw new Error("Prefix is already existed. Please enter a different prefix!");
        }
    };

    return (
        <Modal
            visible={visible}
            title="Create new category"
            okText="Create"
            cancelText="Cancel"
            onCancel={onCancel}
            onOk={() => {
                form
                    .validateFields()
                    .then((values) => {
                        form.resetFields();
                        onCreate(values);
                    })
                    .catch((info) => {
                        console.log('Validate Failed:', info);
                    });
            }}
        >
            <Form
                form={form}
                layout="vertical"
                name="form_in_modal"
            >
                <Form.Item
                    name="categoryName"
                    label="Name"
                    rules={[
                        { required: true, message: 'Name is required!' },
                        { validator: validateCategoryName }
                    ]}
                >
                    <Input />
                </Form.Item>
                <Form.Item
                    name="categoryCode"
                    label="Prefix"
                    rules={[
                        { required: true, message: 'Prefix is required!' },
                        { validator: validateCategoryPrefix }
                    ]}>
                    <Input />
                </Form.Item>
            </Form>
        </Modal>
    );
};
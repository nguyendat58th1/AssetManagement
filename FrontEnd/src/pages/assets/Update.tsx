import {
    Button,
    Col,
    DatePicker,
    Form,
    Input,
    message,
    Radio,
    Space,
} from "antd";
import { useForm } from "antd/lib/form/Form";
import { useEffect, useState } from "react";
import { useHistory, useParams } from "react-router-dom";
import moment from "moment";
import { AssetState, EditAssetModel } from "../../models/Asset";
import { AssetService } from "../../services/AssetService";
import TextArea from "antd/lib/input/TextArea";

export function UpdateAsset() {

    const tailLayout = {
        wrapperCol: {
            span: 35,
            offset: 3,
            pull: 1
        },
    };

    const { assetId } = useParams<any>();

    let history = useHistory()

    const dateFormat = 'YYYY-MM-DD'

    const onFinish = (data: EditAssetModel) => {
        (async () => {
            let service = AssetService.getInstance();
            try {
                await service.update(data, assetId)
                    .then(
                        (res) => {
                            if (res.status === 200) {
                                console.log('Updated Successfully');
                            }
                        }
                    );
                message.success('Updated Successfully');
                history.push('/assets');
            } catch {

            }
        })();
    };

    const onFinishFailed = (errorInfo: any) => {
        message.error(
            'Update Fail !',
        )
        console.log('Failed:', errorInfo)
    }

    const [form] = useForm()
    const [asset, setAsset] = useState<any>();

    let IsAssigned = (state: AssetState) => {
        if (state === AssetState.ASSIGNED) return true;
        return false
    }

    useEffect(() => {
        ; (async () => {
            let service = AssetService.getInstance();
            let asset = await service.getAsset(assetId);
            setAsset(asset);
            form.setFieldsValue({
                assetName: asset.assetName,
                category: asset.category.categoryName,
                specification: asset.specification,
                installedDate: moment(asset.installedDate),
                state: asset.state,
            })
        })()
    }, [])

    return (
        <>
            <Col span={11}>
                <h4>Edit Asset</h4>
            </Col>
            <Col span={22}>
                <Form
                    form={form}
                    onFinish={onFinish}
                    onFinishFailed={onFinishFailed}
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
                        <Input style={{ width: "83%" }} />
                    </Form.Item>

                    <Form.Item
                        name="category"
                        labelCol={{ span: 4 }}
                        wrapperCol={{ span: 11 }}
                        labelAlign="left"
                        label="Category">
                        <Input style={{ width: "83%" }} disabled />
                    </Form.Item>

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
                        <TextArea rows={8} style={{ width: "83%" }} />
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
                        <DatePicker format={dateFormat} style={{ width: "83%" }} />
                    </Form.Item>

                    <Form.Item
                        name="state"
                        labelCol={{ span: 5 }}
                        wrapperCol={{ span: 10 }}
                        labelAlign="left"
                        label="State"
                        hasFeedback
                        rules={[{ required: true, message: "Please select state!" }]}
                    >
                        <Radio.Group disabled={IsAssigned(asset?.state)} style={{ textAlign: "left", float: "left" }}>
                            <Radio style={{ display: "block" }} value={AssetState.AVAILABLE}>Available</Radio>
                            <Radio style={{ display: "block" }} value={AssetState.NOT_AVAILABLE}>Not Available</Radio>
                            <Radio style={{ display: "block" }} value={AssetState.ASSIGNED}>Assigned</Radio>
                            <Radio style={{ display: "block" }} value={AssetState.WAITING_FOR_RECYCLING}>Waiting For Recycling</Radio>
                            <Radio style={{ display: "block" }} value={AssetState.RECYCLED}>Recycled</Radio>
                        </Radio.Group>
                    </Form.Item>

                    <Form.Item
                        {...tailLayout}
                    >
                        <Space>
                            <Button
                                type="primary"
                                htmlType="submit"
                                style={{ backgroundColor: '#e9424d', color: 'white' }}
                            >
                                Save
                            </Button>
                            <Button type="default" danger>
                                <a href="/assets">
                                    Cancel
                                </a>
                            </Button>
                        </Space>
                    </Form.Item>
                </Form>
            </Col>
        </>
    )
}

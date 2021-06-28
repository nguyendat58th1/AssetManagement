import {
  Button,
  Col,
  DatePicker,
  Form,
  Input,
  message,
  Modal,
  Pagination,
  Row,
  Select,
  Table,
  Tag,
  Tooltip,
} from "antd";
import {
  EditOutlined,
  SearchOutlined,
  DeleteOutlined,
  FilterFilled,
  InfoCircleOutlined,
  RedoOutlined,
  ExclamationCircleOutlined,
  PlusCircleOutlined,
} from "@ant-design/icons";
import React, { useEffect, useState } from "react";
import {
  Assignment,
  AssignmentState,
  FilterDate,
} from "../../models/Assignment";
import {
  AssignmentPagedListResponse,
  PaginationParameters,
} from "../../models/Pagination";
import { AssignmentsService } from "../../services/AssignmentService";
import { Link, Redirect } from "react-router-dom";
import { ReturnRequestService } from "../../services/ReturnRequestService";

interface SearchAction {
  action: "filter" | "search" | "filterDate";
  query: number | string | Date;
}
const { Option } = Select;
const { confirm } = Modal;

const dateFormat = "YYYY-MM-DD";
const ADMIN = "Admin";

export function ListAssignments() {
  let [isFetchingData, setIsFetchingData] = useState(false);
  let [isAdminAuthorized] = useState(sessionStorage.getItem("type") === ADMIN);
  let [assignmentPagedList, setAssignmentPagedList] =
    useState<AssignmentPagedListResponse>();
  let [assignmentList, setAssignmentList] = useState<Assignment[]>([]);
  let [filterSelected, setFilterSelected] = useState(false);
  let [filterDateSelected, setFilterDateSelected] = useState(false);
  let [latestSearchAction, setLatestSearchAction] = useState<SearchAction>({
    action: "search",
    query: "",
  });
  let [isDisabledStates, setIsDisabledStates] = useState<boolean[]>([]);

  let assignmentService = AssignmentsService.getInstance();
  let returnRequestService = ReturnRequestService.getInstance();
  useEffect(() => {
    if (isAdminAuthorized) {
      (async () => {
        setIsFetchingData(true);
        let assignmentPagedResponse = await assignmentService.getAssignments();
        let disabledButtonStates: boolean[] = [];

        for (const element of assignmentPagedResponse.items) {
          let associatedRRCount = await returnRequestService.getAssociatedCount(
            element.asset.assetCode
          );
          let isWaitingForAdminDecision = associatedRRCount > 0;
          let isAcceptedState = element.state === AssignmentState.Accepted;
          if (!isAcceptedState) {
            disabledButtonStates.push(true);
          } else {
            disabledButtonStates.push(isWaitingForAdminDecision);
          }
        }

        setIsDisabledStates(disabledButtonStates);
        setAssignmentPagedList(assignmentPagedResponse);
        setAssignmentList(assignmentPagedResponse.items);
        setIsFetchingData(false);
      })();
    }
  }, []);

  async function detailAssignment(id: number) {
    let assignment = await assignmentService.getAssignment(id);
    Modal.info({
      title: `Detail of Assignment `,
      content: (
        <div>
          <p>Asset Code : {assignment.asset.assetCode}</p>
          <p>Asset Name : {assignment.asset.assetName}</p>
          <p>Assigned to : {assignment.assignedToUserName} </p>
          <p>Assigned by : {assignment.assignedByUser.userName}</p>
          <p>Assigned Date : {assignment.assignedDate}</p>
          {assignment.state === AssignmentState.WaitingForAcceptance && (
            <p>
              State :{" "}
              <span style={{ color: "blue" }}>Waiting for acceptance</span>
            </p>
          )}
          {assignment.state === AssignmentState.Accepted && (
            <p>
              State : <span style={{ color: "green" }}>Accepted</span>
            </p>
          )}
          {assignment.state === AssignmentState.Declined && (
            <p>
              State : <span style={{ color: "red" }}>Declined</span>
            </p>
          )}
          <p>Note : {assignment.note}</p>
        </div>
      ),
      onOk() { },
    });
  }

  const createReturnRequest = (index: number, record: Assignment) => {
    confirm({
      title: "Do you want to return this asset?",
      icon: <ExclamationCircleOutlined />,
      onOk: async () => {
        await returnRequestService.create({
          assignmentId: record.id,
        });

        let newIsDisabledStates = [...isDisabledStates];
        newIsDisabledStates[index] = !newIsDisabledStates[index];
        setIsDisabledStates(newIsDisabledStates);
      },
    });
  };

  const columns: any = [
    {
      title: "No.",
      dataIndex: "id",
      key: "id",
      width: 60,
      render: (text: any, record: Assignment, index: number) => {
        return <div>{index + 1}</div>;
      },
    },
    {
      title: "Asset Code",
      dataIndex: "assetCode",
      key: "assetCode",
      sorter: (a: Assignment, b: Assignment) =>
        a.asset.assetCode.localeCompare(b.asset.assetCode),
      render: (text: any, record: Assignment, index: number) => {
        return <div>{record.asset.assetCode}</div>;
      },
      sortDirections: ["ascend", "descend"],
    },
    {
      title: "Asset Name",
      dataIndex: "assetName",
      key: "assetName",
      sorter: (a: Assignment, b: Assignment) =>
        a.asset.assetName.localeCompare(b.asset.assetName),
      render: (text: any, record: Assignment, index: number) => {
        return <div>{record.asset.assetName}</div>;
      },
      sortDirections: ["ascend", "descend"],
    },
    {
      title: "Assigned to",
      dataIndex: "assignedToUserName",
      key: "assignedToUserName",
      sorter: (a: Assignment, b: Assignment) =>
        a.assignedToUserName.localeCompare(b.assignedToUserName),
      render: (text: any, record: Assignment, index: number) => {
        return <div>{record.assignedToUserName}</div>;
      },
      sortDirections: ["ascend", "descend"],
    },
    {
      title: "Assigned by",
      dataIndex: "assignedByUserId",
      key: "assignedByUserId",
      sorter: (a: Assignment, b: Assignment) =>
        a.assignedByUser.userName.localeCompare(b.assignedByUser.userName),
      render: (text: any, record: Assignment, index: number) => {
        return <div>{record.assignedByUser.userName}</div>;
      },
      sortDirections: ["ascend", "descend"],
    },
    {
      title: "Assigned Date",
      dataIndex: "assignedDate",
      key: "assignedDate",
      render: (text: any, record: Assignment, index: number) => {
        return <div>{new Date(record.assignedDate).toLocaleDateString()}</div>;
      },
      sorter: (a: Assignment, b: Assignment) => {
        return (
          new Date(a.assignedDate).getTime() -
          new Date(b.assignedDate).getTime()
        );
      },
      sortDirections: ["ascend", "descend"],
    },
    {
      title: "State",
      dataIndex: "state",
      key: "state",
      render: (text: any, record: Assignment, index: number) => {
        return (
          <>
            {record.state === AssignmentState.Accepted && (
              <Tag color="green" key={record.state}>
                Accepted
              </Tag>
            )}
            {record.state === AssignmentState.WaitingForAcceptance && (
              <Tag color="blue" key={record.state}>
                Waiting for acceptance
              </Tag>
            )}
            {record.state === AssignmentState.Declined && (
              <Tag color="red" key={record.state}>
                Declined
              </Tag>
            )}
          </>
        );
      },
      sorter: (a: Assignment, b: Assignment) => a.state - b.state,
      sortDirections: ["ascend", "descend"],
    },
    {
      title: "",
      dataIndex: "action",
      key: "action",
      render: (text: any, record: Assignment, index: number) => {
        var checkDelete = false;
        var checkUpdate = false;
        if (record.state !== AssignmentState.Accepted) {
          checkDelete = true;
        }
        if (record.state !== AssignmentState.WaitingForAcceptance) {
          checkUpdate = true;
        }

        return (
          <Row>
            <Col>
              <Tooltip placement="top" title="Detail">
                <Button
                  ghost
                  type="link"
                  icon={<InfoCircleOutlined />}
                  onClick={() => detailAssignment(record.id)}
                />
              </Tooltip>
            </Col>

            <Col>
              <Tooltip placement="top" title="Edit">
                <Link to={`/assignments/update/${record.id}`}>
                  <Button
                    type="link"
                    icon={<EditOutlined />}
                    disabled={checkUpdate}
                  />
                </Link>
              </Tooltip>
            </Col>

            <Col>
              <Tooltip placement="top" title="Delete">
                <Button
                  danger
                  type="link"
                  icon={<DeleteOutlined />}
                  onClick={() => deleteAssignment(record.id)}
                  disabled={!checkDelete}
                />
              </Tooltip>
            </Col>
            <Col>
              <Tooltip placement="top" title="Create return request">
                <Button
                  ghost
                  type="link"
                  icon={<RedoOutlined />}
                  disabled={isDisabledStates[index]}
                  onClick={() => createReturnRequest(index, record)}
                />
              </Tooltip>
            </Col>
          </Row>
        );
      },
    },
  ];

  const onPaginationConfigChanged = (page: number, pageSize?: number) => {
    (async () => {
      setIsFetchingData(true);
      let parameters: PaginationParameters = {
        PageNumber: page,
        PageSize: pageSize ?? 10,
      };

      let assignmentPagedResponse: AssignmentPagedListResponse;
      switch (latestSearchAction.action) {
        case "search":
          let searchQuery = latestSearchAction.query as string;
          if (searchQuery.length === 0) {
            assignmentPagedResponse = await assignmentService.getAssignments(
              parameters
            );
          } else {
            assignmentPagedResponse = await assignmentService.searchAssignment(
              searchQuery,
              parameters
            );
          }
          break;
        case "filter":
          let filterQuery = latestSearchAction.query as number;
          assignmentPagedResponse = await assignmentService.filterByState(
            filterQuery,
            parameters
          );
          break;

        case "filterDate":
          let filterDateQuery = latestSearchAction.query as Date;
          let filterDate: FilterDate = {
            year: filterDateQuery.getFullYear(),
            month: filterDateQuery.getMonth(),
            day: filterDateQuery.getDay(),
          };
          assignmentPagedResponse = await assignmentService.filterByDate(
            filterDate,
            parameters
          );
          break;
      }

      setAssignmentPagedList(assignmentPagedResponse);
      setAssignmentList(assignmentPagedResponse.items);
      setIsFetchingData(false);
    })();
  };

  const onSearchButtonClicked = (values: any) => {
    (async () => {
      setIsFetchingData(true);
      let { searchText } = values;
      let assignmentPagedResponse: AssignmentPagedListResponse;

      if (searchText.length === 0) {
        assignmentPagedResponse = await assignmentService.getAssignments();
      } else {
        assignmentPagedResponse = await assignmentService.searchAssignment(
          searchText
        );
      }

      setLatestSearchAction({
        action: "search",
        query: searchText as string,
      });
      setAssignmentPagedList(assignmentPagedResponse);
      setAssignmentList(assignmentPagedResponse.items);
      setIsFetchingData(false);
    })();
  };

  const onFilterStateButtonClicked = (values: any) => {
    (async () => {
      setIsFetchingData(true);

      let { filteredAssignmentByState } = values;
      let assignmentPagedResponse: AssignmentPagedListResponse =
        await assignmentService.filterByState(
          filteredAssignmentByState as number
        );

      setLatestSearchAction({
        action: "filter",
        query: filteredAssignmentByState as number,
      });
      setAssignmentPagedList(assignmentPagedResponse);
      setAssignmentList(assignmentPagedResponse.items);
      setIsFetchingData(false);
    })();
  };

  const onFilterDateButtonClicked = (values: any) => {
    (async () => {
      setIsFetchingData(true);

      let { filteredAssignmentByDate } = values;

      if (filteredAssignmentByDate === null) {
        message.error("Date is empty");
        setIsFetchingData(false);
      } else {
        let filterDate: FilterDate = {
          year: JSON.parse(
            filteredAssignmentByDate._d.getFullYear().toString()
          ),
          month:
            JSON.parse(filteredAssignmentByDate._d.getMonth().toString()) + 1,
          day: JSON.parse(filteredAssignmentByDate._d.getDate().toString()),
        };
        let assignmentPagedResponse: AssignmentPagedListResponse =
          await assignmentService.filterByDate(filterDate);

        setLatestSearchAction({
          action: "filterDate",
          query: filteredAssignmentByDate as Date,
        });
        setAssignmentPagedList(assignmentPagedResponse);
        setAssignmentList(assignmentPagedResponse.items);
        setIsFetchingData(false);
      }
    })();
  };

  function deleteAssignment(id: number) {
    confirm({
      title: "Do you want to delete this assignment?",
      icon: <ExclamationCircleOutlined />,
      onOk() {
        try {
          assignmentService.delete(id);
          message.success("Deleted Successfully");
          setAssignmentList((userId: any[]) =>
            userId.filter((item) => item.id !== id)
          );
        } catch {
          message.error("Something went wrong");
        }
      },
      onCancel() {
        console.log("Cancel");
      },
    });
  }

  return (
    <>
      {!isAdminAuthorized && <Redirect to="/401-access-denied" />}
      {isAdminAuthorized && assignmentPagedList !== undefined && (
        <>
          <Row>
            <Col span={5}>
              <Form onFinish={onFilterStateButtonClicked}>
                <Row justify="start">
                  <Col span={18}>
                    <Form.Item
                      name="filteredAssignmentByState"
                      className="no-margin-no-padding"
                    >
                      <Select
                        placeholder="Select assignment's state"
                        style={{ width: "100%" }}
                        onSelect={() => setFilterSelected(true)}
                        disabled={isFetchingData}
                      >
                        <Option key="waitingForAcceptance" value={0}>
                          Waiting for acceptance
                        </Option>
                        <Option key="Accepted" value={1}>
                          Accepted
                        </Option>
                        <Option key="Declined" value={2}>
                          Declined
                        </Option>
                      </Select>
                    </Form.Item>
                  </Col>

                  <Col offset={1}>
                    <Form.Item className="no-margin-no-padding">
                      <Button
                        size="middle"
                        icon={<FilterFilled />}
                        htmlType="submit"
                        disabled={!filterSelected || isFetchingData}
                      />
                    </Form.Item>
                  </Col>
                </Row>
              </Form>
            </Col>

            <Col span={5}>
              <Form onFinish={onFilterDateButtonClicked}>
                <Row justify="start">
                  <Col span={15}>
                    <Form.Item
                      name="filteredAssignmentByDate"
                      className="no-margin-no-padding"
                    >
                      <DatePicker
                        onChange={() => setFilterDateSelected(true)}
                        format={dateFormat}
                      />
                    </Form.Item>
                  </Col>

                  <Col offset={1}>
                    <Form.Item className="no-margin-no-padding">
                      <Button
                        size="middle"
                        icon={<FilterFilled />}
                        htmlType="submit"
                        disabled={!filterDateSelected || isFetchingData}
                      />
                    </Form.Item>
                  </Col>
                </Row>
              </Form>
            </Col>

            <Col span={6} offset={3}>
              <Form
                onFinish={onSearchButtonClicked}
                initialValues={{
                  searchText: "",
                }}
              >
                <Row justify="end">
                  <Col span={18}>
                    <Form.Item
                      name="searchText"
                      className="no-margin-no-padding"
                    >
                      <Input
                        allowClear
                        disabled={isFetchingData}
                        style={{ width: "100%" }}
                        placeholder="e.g. Laptop/LA0001/binhnv"
                      />
                    </Form.Item>
                  </Col>
                  <Col offset={1}>
                    <Form.Item className="no-margin-no-padding">
                      <Button
                        size="middle"
                        icon={<SearchOutlined />}
                        type="primary"
                        htmlType="submit"
                        disabled={isFetchingData}
                      />
                    </Form.Item>
                  </Col>
                </Row>
              </Form>
            </Col>
            <Col span={4} offset={1}>
              <Link to="/assignments/create">
                <Button
                  style={{
                    width: "100%",
                    backgroundColor: "#e9424d",
                    border: "#e9424d",
                  }}
                  type="primary"
                  icon={<PlusCircleOutlined />}
                >
                  Create new assignment
                </Button>
              </Link>
            </Col>
          </Row>

          <Table
            style={{
              margin: "1.25em 0 1.25em 0",
            }}
            dataSource={assignmentList}
            columns={columns}
            scroll={{ y: 400 }}
            pagination={false}
            loading={isFetchingData}
          />
          <Row justify="center">
            <Col>
              <Pagination
                disabled={isFetchingData}
                total={assignmentPagedList.totalCount}
                onChange={onPaginationConfigChanged}
                showTotal={(total: number) => `Total: ${total} result(s)`}
                pageSizeOptions={["10", "20", "50"]}
                showSizeChanger
              />
            </Col>
          </Row>
        </>
      )}
    </>
  );
}

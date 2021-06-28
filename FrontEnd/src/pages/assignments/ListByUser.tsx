import React, { useEffect, useState } from "react";
import { Button, Col, Modal, Row, Table, Tag } from "antd";
import { Assignment, AssignmentState } from "../../models/Assignment";
import { AssignmentsService } from "../../services/AssignmentService";
import {
  InfoCircleOutlined,
  RedoOutlined,
  ExclamationCircleOutlined,
  CheckOutlined,
  CloseOutlined,
} from "@ant-design/icons";
import { ReturnRequestService } from "../../services/ReturnRequestService";

export enum ResponeAction {
  NoAction,
  Accept,
  Decline,
  UndoRespone,
}

const { confirm } = Modal;

export function ListAssignmentsForEachUser() {
  let [assignmentList, setAssignmentList] = useState<Assignment[]>([]);
  let [isDisabledStates, setIsDisabledStates] = useState<boolean[]>([]);

 
  let assignmentService = AssignmentsService.getInstance();
  let returnRequestService = ReturnRequestService.getInstance();

  //Respone to Assignment
  const [visible, setVisible] = React.useState(false);
  const [confirmLoading, setConfirmLoading] = React.useState(false);
  const [modalText, setModalText] = React.useState("");
  const [responeAction, setResponeAction] = React.useState(
    ResponeAction.NoAction
  );
  const [assignmentId, setAssignmentId] = React.useState(0);

  const showModal = () => {
    setVisible(true);
  };

  const handleOk = () => {
    if (responeAction === ResponeAction.NoAction) {
      setVisible(false);
      setConfirmLoading(false);
    }
    if (responeAction === ResponeAction.Accept) {
      setConfirmLoading(true);
      assignmentService.acceptAssignment(assignmentId);
    }
    if (responeAction === ResponeAction.Decline) {
      setConfirmLoading(true);
      assignmentService.declineAssignment(assignmentId);
    }
    if (responeAction === ResponeAction.UndoRespone) {
      setConfirmLoading(true);
      assignmentService.undoResponeAssignment(assignmentId);
    }
    setConfirmLoading(false);
    setVisible(false);
    setAssignmentId(0);
    setResponeAction(ResponeAction.NoAction);
    window.location.reload();
  };

  const handleCancel = () => {
    setVisible(false);
    setAssignmentId(0);
    setResponeAction(ResponeAction.NoAction);
  };

  useEffect(() => {
    (async () => {
      let listAssignments = await assignmentService.getAllForEachUser(
        JSON.parse(sessionStorage.getItem("id")!)
      );
      let disabledButtonStates: boolean[] = [];

      for (const element of listAssignments) {
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
      setAssignmentList(listAssignments);
    })();
  }, []);

  async function detailAssignment(id: number) {
    let assignment = await assignmentService.getAssignment(id);
    Modal.info({
      title: `Detail of Assignment`,
      content: (
        <div>
          <p>Asset Code : {assignment.asset.assetCode}</p>
          <p>Asset Name : {assignment.asset.assetName}</p>
          <p>Assigned to : {assignment.assignedToUserName}</p>
          <p>Assigned by : {assignment.assignedByUser.userName}</p>
          <p>Assigned Date : {assignment.assignedDate}</p>
          {assignment.state === AssignmentState.WaitingForAcceptance && <p>State : <span style={{color : 'blue'}}>Waiting for acceptance</span></p>}
          {assignment.state === AssignmentState.Accepted &&<p>State : <span style={{color : 'green'}}>Accepted</span></p>}
          {assignment.state === AssignmentState.Declined &&<p>State : <span style={{color : 'red'}}>Declined</span></p>}
          <p>Note : {assignment.note}</p>
        </div>
      ),
      onOk() {},
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
 

   //Respone to assignment
   const acceptAssignment = (id: number) => {
    setAssignmentId(id);
    setResponeAction(ResponeAction.Accept);
    setModalText("Do you want to accept to this assignment?");
    showModal();
  };

  const declineAssignment = (id: number) => {
    setAssignmentId(id);
    setResponeAction(ResponeAction.Decline);
    setModalText("Do you want to decline to this assignment?");
    showModal();
  };

  const columns: any = [
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
      sorter: (a: Assignment, b: Assignment) => a.assignedToUserName.localeCompare(b.assignedToUserName),
      render: (text: any, record: Assignment, index: number) => {
        return (
          <div>
         {record.assignedToUserName}
          </div>
        );
      },
      sortDirections: ["ascend", "descend"],
      
    },
    {
      title: "Assigned by",
      dataIndex: "assignedByUserId",
      key: "assignedByUserId",
      sorter: (a: Assignment, b: Assignment) => {
        a.assignedByUser.userName.localeCompare(b.assignedByUser.userName);
      },
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
        return (
          <Row>
            <Col>
              <Button
                ghost
                type="link"
                icon={<InfoCircleOutlined />}
                title="Detail"
                onClick={() => detailAssignment(record.id)}
              />
            </Col>
         
            {/* Respone to Assignment */}
            <Col>
              <Button
                ghost
                type="link"
                title="Accept"
                icon={<CheckOutlined />}
                disabled={record.state !== AssignmentState.WaitingForAcceptance}
                onClick={() => acceptAssignment(record.id)}
              />
            </Col>
            <Col>
              <Button
                ghost
                type="link"
                icon={<CloseOutlined />}
                title="Decline"
                disabled={record.state !== AssignmentState.WaitingForAcceptance}
                onClick={() => declineAssignment(record.id)}
              />
            </Col>
            <Col>
              <Button
                ghost
                type="link"
                title="Request To Return"
                icon={<RedoOutlined />}
                disabled={isDisabledStates[index]}
                onClick={() => createReturnRequest(index, record)}
              />
            </Col>
          </Row>
        );
      },
    },
  ];
  return (
    <>
      <Table
        style={{
          margin: "1.25em 0 1.25em 0",
        }}
        dataSource={assignmentList}
        columns={columns}
        scroll={{ y: 400 }}
        pagination={false}
      />
      <Modal
        title="Are you sure?"
        visible={visible}
        onOk={handleOk}
        confirmLoading={confirmLoading}
        onCancel={handleCancel}
      >
        <p>{modalText}</p>
      </Modal>
    </>
  );
}

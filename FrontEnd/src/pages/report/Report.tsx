import { Button, Table } from "antd";
import { useState, useEffect } from "react";
import { Redirect } from "react-router-dom";
import { ReportModel } from "../../models/ReportModel";
import { reportService } from "../../services/report.service";

export function ReportView() {
  let [isAdminAuthorized] = useState(
    sessionStorage.getItem("type") === "Admin"
  );
  let [isFetchingData, setIsFetchingData] = useState(false);
  let [reportList, setReportList] = useState<ReportModel[]>([]);

  const handleExport = async () => {
    await reportService.exportExcel();
    alert("Your report file save in C:/Reports");
  };

  useEffect(() => {
    if (isAdminAuthorized) {
      (async () => {
        setIsFetchingData(true);
        let report = await reportService.getReport();

        setReportList(report);
        setIsFetchingData(false);
      })();
    }
  }, []);

  const columns: any = [
    {
      title: "Category Name",
      dataIndex: "categoryName",
      key: "categoryName",
      sorter: (a: ReportModel, b: ReportModel) =>
        a.categoryName.localeCompare(b.categoryName),
      sortDirections: ["descend"],
    },
    {
      title: "Total",
      dataIndex: "total",
      key: "total",
      sorter: (a: ReportModel, b: ReportModel) => b.total - a.total,
      sortDirections: ["descend"],
    },
    {
      title: "Assigned",
      dataIndex: "assigned",
      key: "assigned",
      sorter: (a: ReportModel, b: ReportModel) => b.assigned - a.assigned,
      sortDirections: ["descend"],
    },
    {
      title: "Available",
      dataIndex: "available",
      key: "available",
      sorter: (a: ReportModel, b: ReportModel) => b.available - a.available,
      sortDirections: ["descend"],
    },
    {
      title: "NotAvailable",
      dataIndex: "notAvailable",
      key: "notAvailable",
      sorter: (a: ReportModel, b: ReportModel) =>
        b.notAvailable - a.notAvailable,
      sortDirections: ["descend"],
    },
    {
      title: "WaitingForRecycling",
      dataIndex: "waitingForRecycling",
      key: "waitingForRecycling",
      sorter: (a: ReportModel, b: ReportModel) =>
        b.waitingForRecycling - a.waitingForRecycling,
      sortDirections: ["descend"],
    },
    {
      title: "Recycled",
      dataIndex: "recycled",
      key: "recycled",
      sorter: (a: ReportModel, b: ReportModel) => b.recycled - a.recycled,
      sortDirections: ["descend"],
    },
  ];

  return (
    <>
      <h5 style={{ color: "rgb(207, 35, 56)", textAlign: "left" }}>Report</h5>
      <Button
        style={{
          float: "right",
          backgroundColor: "rgb(207, 35, 56)",
          color: "white",
          margin: "1.25em 0 1.25em 0",
          borderRadius: "8px",
        }}
        onClick={handleExport}
      >
        Export
      </Button>
      {!isAdminAuthorized && <Redirect to="/401-access-denied" />}
      {isAdminAuthorized !== undefined && (
        <>
          <Table
            style={{
              margin: "1.25em 0 1.25em 0",
            }}
            dataSource={reportList}
            columns={columns}
            scroll={{ y: 400 }}
            pagination={false}
            loading={isFetchingData}
          />
        </>
      )}
    </>
  );
}

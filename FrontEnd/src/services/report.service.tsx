import { authHeader } from "../helpers/auth-header";
import { handleResponse } from "../helpers/handle-response";
import { authenticationService } from "./authentication.service";

export enum Location {
  HaNoi = 0,
  HoChiMinh = 1,
}

export const reportService = {
  getReport,
  exportExcel,
};

async function getReport() {
  const requestOptions = { method: "GET", headers: authHeader() };
  var response = await fetch(
    `https://localhost:5001/api/reports/${authenticationService.currentUserValue.location}`,
    requestOptions
  );
  return handleResponse(response);
}

async function exportExcel() {
  const requestOptions = { method: "GET", headers: authHeader() };
  var response = await fetch(
    `https://localhost:5001/api/Reports/ExportXls/${authenticationService.currentUserValue.location}`,
    requestOptions
  );
  return handleResponse(response);
}

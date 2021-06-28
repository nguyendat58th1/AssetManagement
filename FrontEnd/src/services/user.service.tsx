import { authHeader } from "../helpers/auth-header";
import { handleResponse } from "../helpers/handle-response";

export const userService = {
  getAll,
  getById,
  changePassword,
};

function getAll() {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`https://localhost:5001/api/users`, requestOptions).then(
    handleResponse
  );
}

function getById(id: number) {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`https://localhost:5001/api/users/${id}`, requestOptions).then(
    handleResponse
  );
}

function changePassword(id: number, oldpassword: string, newpassword: string) {
  const requestOptions = {
    method: "PUT",
    headers: authHeader(),
    body: JSON.stringify({
      oldPassword: oldpassword,
      newPassword: newpassword,
    }),
  };
  return fetch(
    `https://localhost:5001/api/users/change-password/${id}`,
    requestOptions
  ).then(handleResponse);
}

import { BehaviorSubject } from "rxjs";
import { handleResponse } from "../helpers/handle-response";

const currentUserSubject = new BehaviorSubject(
  JSON.parse(localStorage.getItem("currentUser") || "{}")
);

export const authenticationService = {
  login,
  logout,
  currentUser: currentUserSubject.asObservable(),
  get currentUserValue() {
    return currentUserSubject.value;
  },
};

async function login(username: string, password: string) {
  const requestOptions = {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ userName: username, password: password }),
  };

  const response = await fetch(
    `https://localhost:5001/Authentication/authenticate`,
    requestOptions
  );
  const user = await handleResponse(response);
  localStorage.setItem("currentUser", JSON.stringify(user));
  currentUserSubject.next(user);
  return user;
}

function logout() {
  localStorage.removeItem("currentUser");
  currentUserSubject.next(null);
}

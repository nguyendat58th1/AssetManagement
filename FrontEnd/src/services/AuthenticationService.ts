import { LoggedInUser, UserLogin } from "../models/UserModel";
import { HttpClient } from "./HttpClient";

export class AuthenticationService extends HttpClient{
  private static instance?: AuthenticationService;

  private constructor() {
    super('https://localhost:5001');
  }

  public static getInstance = () : AuthenticationService => {
    if (AuthenticationService.instance === undefined) {
      AuthenticationService.instance = new AuthenticationService();
    }

    return AuthenticationService.instance;
  } 

  public login = (user: UserLogin) => this.instance.post<LoggedInUser>("/Authentication/authenticate", user);

  //todo Only need to delete the token from the session storage
  // public logout = ()=>this.instance.post("/logout");
}

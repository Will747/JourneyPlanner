import axios from "axios";
import { OverwriteUserData } from "./reducers/user";

function login(dispatch)
{
    axios.get("/api/v1/login").then(
        loggedIn => {
          if (loggedIn.data)
          {
            axios.get("/api/v1/user").then(
              userResponse => dispatch(OverwriteUserData(userResponse.data))
            )
          }
      }
      )
}

export default login;
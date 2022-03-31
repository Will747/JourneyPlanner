import { configureStore } from "@reduxjs/toolkit";
import routeReducer from "./reducers/route";
import userReducer from "./reducers/user";

export default configureStore({
  reducer: {
    route: routeReducer,
    user: userReducer
  },
});

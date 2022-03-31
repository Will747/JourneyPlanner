import { createSlice } from "@reduxjs/toolkit";

export const user = createSlice({
  name: "user",
  initialState: {
    User:
    {
        id: 0,
        username: null,
        email: null
    },
    Routes: []
  },
  reducers: {
    OverwriteUserData: (state, action) => {
         state.User = action.payload;
    },
    OverwriteRoutes: (state, action) => {
      state.Routes = action.payload;
    },
    AddRoute: (state, action) => {
      state.User = action.payload;
    },
    RemoveRoute: (state, action) => {
      state.Routes = [...state.Routes.slice(0, action.payload) , ...state.Routes.slice(action.payload + 1)];
    },
  },
});

export const { 
  OverwriteUserData, 
  OverwriteRoutes, 
  AddRoute,
  RemoveRoute
} = user.actions;

export const selectUser = (state) => state.user.User;
export const selectRoutes = (state) => state.user.Routes;

export default user.reducer;
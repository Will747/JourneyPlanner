import { createSlice } from "@reduxjs/toolkit";

export const route = createSlice({
  name: "route",
  initialState: {
    userInput: {
      stops: [null, null],
      type: "Single",
      algorithm: "Distance"
    },
    routes: [
      {
      _stations: null,
      lines: [null],
      totalDistance: 0
      }
  ],
  },
  reducers: {
    overwriteUserInput: (state, action) => {
      state.userInput = action.payload;
    },
    overwriteUserInputStops: (state, action) => {
      state.userInput.stops = action.payload;
    },
    overwriteUserInputType: (state, action) => {
      state.userInput.type = action.payload;
    },
    overwriteUserInputAlgorithm: (state, action) => {
      state.userInput.algorithm = action.payload;
    },
    overwriteRoutes: (state, action) => {
      state.routes = action.payload;
    }
  },
});

export const { overwriteUserInput,
               overwriteUserInputStops, 
               overwriteUserInputAlgorithm, 
               overwriteUserInputType, 
               overwriteRoutes } = route.actions;

export const selectUserInput = (state) => state.route.userInput;
export const selectUserInputType = (state) => state.route.userInput.type;
export const selectRoutes = (state) => state.route.routes;

export default route.reducer;

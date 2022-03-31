import React from "react";

import ToFrom from "./ToFrom";

function Single({ data, handleChange, stations }) {
  const handleTo = (newValue) => {
    handleChange([newValue, data[1]]);
  };

  const handleFrom = (newValue) => {
    handleChange([data[0], newValue]);
  };

  return (
    <>
      <ToFrom
        To={data[0]}
        From={data[1]}
        handleToChange={handleTo}
        handleFromChange={handleFrom}
      />
    </>
  );
}

export default Single;

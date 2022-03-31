import axios from "axios";

function search(value, setResult) {
  if (value.length >= 2) {
    axios.get("/api/v1/stations/search/" + value).then((response) => {
      setResult(response.data);
    });
  } else {
    setResult([]);
  }
}

export default search;

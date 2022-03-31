import React, { useState, useEffect } from "react";
import { useSelector } from "react-redux";
import axios from "axios";

import Accordion from "@material-ui/core/Accordion";
import AccordionSummary from "@material-ui/core/AccordionSummary";
import AccordionDetails from "@material-ui/core/AccordionDetails";
import List from "@material-ui/core/List";
import ListItem from "@material-ui/core/ListItem";
import ListItemText from "@material-ui/core/ListItemText";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";

import { selectUserInput } from "../../reducers/route";

import Tile from "../Tile";
import classes from "./RoutesBox.module.css";

function DirectServices() {
  const userInput = useSelector(selectUserInput);
  const [services, setServices] = useState([]);

  useEffect(() => {
    axios.post("/api/v1/stations/directService", {
      Stops: userInput.stops,
    }).then((response) => setServices(response.data));
  }, [userInput]);

  return (
    <Tile>
      {services.length == 0  ? (
        <h5>No Direct Services</h5>
      ) : (
        <>
          <h4>Direct Services:</h4>
          <div className={classes.List}>
            {services.map((service) => (
              <Accordion>
                <AccordionSummary expandIcon={<ExpandMoreIcon />}>
                  <strong>{service.departureTime}</strong>
                  &nbsp; - &nbsp;
                  {service.origin + " to " + service.destination}
                </AccordionSummary>
                <AccordionDetails>
                  <List>
                    <ListItem>
                      <ListItemText primary={"Operator: " + service.atocName} />
                    </ListItem>
                    <ListItem>
                      <ListItemText primary={"Platform: " + service.platform} />
                    </ListItem>
                    <ListItem>
                      <ListItemText
                        primary={"Departure Time: " + service.departureTime}
                      />
                    </ListItem>
                    <ListItem>
                      <ListItemText primary={"Origin: " + service.origin} />
                    </ListItem>
                    <ListItem>
                      <ListItemText
                        primary={"Destination: " + service.destination}
                      />
                    </ListItem>
                    <ListItem>
                      <ListItemText primary={"Train ID: " + service.trainId} />
                    </ListItem>
                  </List>
                </AccordionDetails>
              </Accordion>
            ))}
          </div>
        </>
      )}
    </Tile>
  );
}

export default DirectServices;

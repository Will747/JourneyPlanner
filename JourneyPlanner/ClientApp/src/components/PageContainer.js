import React from "react";
import classes from "./PageContainer.module.css";
import { Container, Grid } from "@material-ui/core";

function PageContainer(props) {
  return (
    <Container className={classes.container}>
      <Grid container justify="center">
          {props.children}
      </Grid>
    </Container>
  );
}

export default PageContainer;

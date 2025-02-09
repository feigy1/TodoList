import React from "react";
import AppHeader from "./AppHeader";
import CssBaseline from "@mui/material/CssBaseline";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";

function Copyright() {
    return (
      <Typography variant="body2" color="text.secondary" align="center">
        {"Copyright © "}
          Auth Sample
        {new Date().getFullYear()}
        {"."}
      </Typography>
    );
  }

// const theme = createTheme();
const theme = createTheme({
  direction: 'rtl', // הוספת כיוון RTL
});
const Layout = ({ children }) => {
  return (
    <>
      <div>
        <AppHeader></AppHeader>
      </div>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        <main>{children}</main>
        <Box sx={{ bgcolor: "background.paper", pt: 6, pb: 6 }} component="footer">
        <Typography
          variant="subtitle1"
          align="center"
          color="text.secondary"
          component="p"
        >
        </Typography>
        <Copyright />
      </Box>
      </ThemeProvider>
    </>
  );
};

export default Layout;

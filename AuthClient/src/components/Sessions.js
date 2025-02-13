import * as React from "react";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";
import { useEffect, useState } from "react";
import Service from "../AuthService";

export default function Sessions() {
  const [sessions, setSessions] = useState([]);

  useEffect(() => {
    Service.getPrivate().then((data) => {
        console.log('sessions', data)
      setSessions(data);
    });
  }, []);

  return (
    <TableContainer component={Paper}>
      <Table sx={{ minWidth: 750 }} aria-label="simple table">
        <TableHead>
          <TableRow>
            <TableCell>מזהה</TableCell>
            <TableCell>מזהה משתמש</TableCell>
            <TableCell>תאריך ושעה</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
        {/* {sessions && sessions.length > 0 ? (
            sessions.map((row) => (
              <TableRow
                key={row.number}
                sx={{ "&:last-child td, &:last-child th": { border: 0 } }}
              >
                <TableCell component="th" scope="row">
                  {row.number}
                </TableCell>
              
                <TableCell>{row.userId}</TableCell>
                <TableCell>{row.date}</TableCell>
              </TableRow>
            ))
          ) : (
            <TableRow>
              <TableCell colSpan={3} align="center">
                אין התחברויות קודמות
              </TableCell>
            </TableRow>
          )} */}
        </TableBody>
      </Table>
    </TableContainer>
  );
}

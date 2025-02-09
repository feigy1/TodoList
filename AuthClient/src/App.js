import { Route, Routes } from "react-router-dom";
import { BrowserRouter as Router } from 'react-router-dom';
import AppRoutes from "./AppRoutes";
import Layout from "./components/Layout";
import { CacheProvider } from '@emotion/react';
import React, { useEffect, useState } from 'react';
import service from './TaskService';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import createCache from '@emotion/cache';
import rtlPlugin from 'stylis-plugin-rtl';
import { prefixer } from 'stylis';
const cacheRtl = createCache({
  key: 'muirtl',
  stylisPlugins: [prefixer, rtlPlugin],
});
const theme = createTheme({
  direction: 'rtl', 
});

function App() {
  const [newTodo, setNewTodo] = useState("");
  const [todos, setTodos] = useState([]);

  async function getTodos() {
    const todos = await service.getTasks();
    setTodos(todos);
  }

  async function createTodo(e) {
    e.preventDefault();
    await service.addTask(newTodo);
    setNewTodo("");
    await getTodos();
  }

  async function updateCompleted(todo, isComplete) {
    await service.setCompleted(todo.id, isComplete);
    await getTodos();
  }

  async function deleteTodo(id) {
    await service.deleteTask(id);
    await getTodos();
  }

  return (
    <div className="App">
      <CacheProvider value={cacheRtl}>
        <Router>
          <Layout>
            <Routes>
              {AppRoutes.map((route, index) => {
                const { element, ...rest } = route;
                return <Route key={index} {...rest} element={element} />;
              })}
            </Routes>
          </Layout>
        </Router>
      </CacheProvider>
    </div>
  );

}

export default App;
import React from 'react';
import Public from "./components/Public";
import Private from "./components/Private";
import Home from "./components/Home";
import Login from "./components/Login";
import Register from './components/Register';
import Tasks from './components/tasks';

const AppRoutes = [
    {
        index: true,
        element: <Home />
    },
    {
        path: '/public',
        element: <Public />
    },
    {
        path: '/private',
        element: <Private />
    },
    {
        path: '/login',
        element: <Login />
    },
    {
        path: '/register',
        element: <Register />
    },
    {
        path: '/tasks',
        element: <Tasks />
    }
];

export default AppRoutes;

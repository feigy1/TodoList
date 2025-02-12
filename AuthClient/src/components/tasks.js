import React from "react";
import Sessions from "./Sessions";
import Box from "@mui/material/Box";
import Container from "@mui/material/Container";
import Typography from "@mui/material/Typography";
import service from "../TaskService";
import { useEffect, useState } from "react";

export default function Tasks() {
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

    useEffect(() => {
        const token = localStorage.getItem("access_token");
        if (token) {
            getTodos();
        }
    }, []);

    return (
        <section className="todoapp">
            <header className="header">
                <h1>todos</h1><br></br>
                <form onSubmit={createTodo}>
                    <input
                        className="new-todo"
                        placeholder="Well, let's take on the day"
                        value={newTodo}
                        onChange={(e) => setNewTodo(e.target.value)}
                    />
                </form>
            </header>
            <section className="main" style={{ display: "block" }}>
                <ul className="todo-list">
                    {todos.map((todo) => {
                        return (
                            <li
                                className={todo.isComplete ? "completed" : ""}
                                key={todo.id}
                            >
                                <div className="view">
                                    <input
                                        className="toggle"
                                        type="checkbox"
                                        defaultChecked={todo.isComplete}
                                        onChange={(e) =>
                                            updateCompleted(todo, e.target.checked)
                                        }
                                    />
                                    <label>{todo.name}</label>
                                    <button
                                        className="destroy"
                                        onClick={() => deleteTodo(todo.id)}
                                    ></button>
                                </div>
                            </li>
                        );
                    })}
                </ul>
            </section>
        </section>
    )
}

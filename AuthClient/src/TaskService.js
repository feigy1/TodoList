import axios from 'axios';

axios.interceptors.response.use(
  response => response, 
  error => {
    console.error("API Error:", error.response ? error.response.data : error.message);
    return Promise.reject(error); 
  }
);
 
const apiUrl = 'https://authserver-344d.onrender.com/'; 

axios.defaults.baseURL = apiUrl;
axios.defaults.headers['Content-Type'] = 'application/json';

export default {
  getTasks: async () => {
    try {
      const result = await axios.get("/tasks"); 
      return result.data;
    } catch (error) {
      console.error("Error fetching tasks:", error);
      throw error;
    }
  },

  addTask: async (name) => {
    try {
      const result = await axios.post("/tasks", { name });
      return result.data; 
    } catch (error) {
      console.error("Error adding task:", error);
      throw error;
    }
  },

  setCompleted: async (id, isComplete ) => {
    try {
      const result = await axios.put(`/tasks/${id}`, { isComplete });
      return result.data; 
    } catch (error) {
      console.error("Error updating task:", error);
      throw error;
    }
  },

  deleteTask: async (id) => {
    try {
      await axios.delete(`/tasks/${id}`);
      return { success: true }; 
    } catch (error) {
      console.error("Error deleting task:", error);
      throw error;
    }
  }
};

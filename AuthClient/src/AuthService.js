import axios from "axios";
import { jwtDecode } from 'jwt-decode';


axios.defaults.baseURL = process.env.REACT_APP_API_URL;
console.log('process.env.API_URL', process.env.REACT_APP_API_URL)
setAuthorizationBearer();

function saveAccessToken(authResult) {
  localStorage.setItem("access_token", authResult.token);
  console.log('process.env.API_URL', process.env.REACT_APP_API_URL)
  setAuthorizationBearer();
}

function setAuthorizationBearer() {
  const accessToken = localStorage.getItem("access_token");
  if (accessToken) {
    axios.defaults.headers.common["Authorization"] = `Bearer ${accessToken}`;
  }
}

axios.interceptors.response.use(
  function(response) {
    return response;
  },
  function(error) {
    if (error.response.status === 401) {
      return (window.location.href = "/register");
    }
    return Promise.reject(error);
  }
);

export default {
  getLoginUser: () => {
    const accessToken = localStorage.getItem("access_token");
    if (accessToken) {
      return jwtDecode(accessToken);
    }
    return null;
  },

  logout:()=>{
    localStorage.setItem("access_token", "");
  },

  register: async (name, password) => {
    const res = await axios.post("/api/register", { name, password });
    saveAccessToken(res.data);
  },

  login: async (name, password) => {
    const res = await axios.post("/api/login", { name, password });
    // console.log("Token received:", res.data.token);
    saveAccessToken(res.data);
  },

  getPublic: async () => {
    const res = await axios.get("/api/Public");
    return res.data;
  },

  getPrivate: async () => {
    const res = await axios.get("/api/Private");
    return res.data;
  },
};

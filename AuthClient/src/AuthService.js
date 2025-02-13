// import axios from "axios";
// import { jwtDecode } from 'jwt-decode';


// axios.defaults.baseURL = process.env.REACT_APP_API_URL;
// console.log('process.env.API_URL', process.env.REACT_APP_API_URL)
// setAuthorizationBearer();

// function saveAccessToken(authResult) {
//   localStorage.setItem("access_token", authResult.token);
//   console.log('process.env.API_URL', process.env.REACT_APP_API_URL)
//   setAuthorizationBearer();
// }

// function setAuthorizationBearer() {
//   const accessToken = localStorage.getItem("access_token");
//   if (accessToken) {
//     axios.defaults.headers.common["Authorization"] = `Bearer ${accessToken}`;
//   }
// }

// axios.interceptors.response.use(
//   function(response) {
//     return response;
//   },
//   function(error) {
//     if (error.response.status === 401) {
//       return (window.location.href = "/register");
//     }
//     return Promise.reject(error);
//   }
// );

// export default {
//   getLoginUser: () => {
//     const accessToken = localStorage.getItem("access_token");
//     if (accessToken) {
//       return jwtDecode(accessToken);
//     }
//     return null;
//   },

//   logout:()=>{
//     localStorage.setItem("access_token", "");
//   },

//   register: async (name, password) => {
//     const res = await axios.post("/api/register", { name, password });
//     saveAccessToken(res.data);
//   },

//   login: async (name, password) => {
//     const res = await axios.post("/api/login", { name, password });
//     // console.log("Token received:", res.data.token);
//     saveAccessToken(res.data);
//   },

//   getPublic: async () => {
//     const res = await axios.get("/api/Public");
//     return res.data;
//   },

//   getPrivate: async () => {
//     const res = await axios.get("/api/Private");
//     return res.data;
//   },
// };



import axios from "axios";
import { jwtDecode } from 'jwt-decode';
 // ודא שהייבוא תואם לגרסה שבה אתה משתמש

// הגדרת כתובת הבסיס של Axios
axios.defaults.baseURL = process.env.REACT_APP_API_URL;
console.log('process.env.REACT_APP_API_URL:', process.env.REACT_APP_API_URL);

setAuthorizationBearer();

function saveAccessToken(authResult) {
  console.log('saveAccessToken called with authResult:', authResult);
  localStorage.setItem("access_token", authResult.token);
  setAuthorizationBearer();
}

function setAuthorizationBearer() {
  const accessToken = localStorage.getItem("access_token");
  console.log('setAuthorizationBearer called. accessToken:', accessToken);
  if (accessToken) {
    axios.defaults.headers.common["Authorization"] = `Bearer ${accessToken}`;
    console.log('Authorization header set:', axios.defaults.headers.common["Authorization"]);
  } else {
    console.log('No access token found in localStorage.');
  }
}

axios.interceptors.response.use(
  function(response) {
    console.log('Response received:', response);
    return response;
  },
  function(error) {
    console.log('Error response received:', error.response);
    if (error.response && error.response.status === 401) {
      console.log('Unauthorized (401) error. Redirecting to /register.');
      window.location.href = "/register";
    }
    return Promise.reject(error);
  }
);

export default {
  getLoginUser: () => {
    const accessToken = localStorage.getItem("access_token");
    console.log('getLoginUser called. accessToken:', accessToken);
    if (accessToken) {
      try {
        const decoded = jwtDecode(accessToken);
        console.log('Decoded token:', decoded);
        return decoded;
      } catch (error) {
        console.log('Error decoding token:', error);
        return null;
      }
    }
    return null;
  },

  logout: () => {
    console.log('logout called.');
    localStorage.removeItem("access_token");
  },

  register: async (name, password) => {
    console.log('register called with name:', name);
    try {
      const res = await axios.post("/api/register", { name, password });
      console.log('Registration response:', res);
      saveAccessToken(res.data);
    } catch (error) {
      if (error.response) {
        // השרת החזיר תגובה עם קוד שגיאה
        console.error('Response error:', error.response.status, error.response.data);
      } else if (error.request) {
        // הבקשה נשלחה אך לא התקבלה תגובה
        console.error('No response received:', error.request);
      } else {
        // שגיאה בהגדרת הבקשה
        console.error('Error setting up request:', error.message);
      }
    }
  },

  login: async (name, password) => {
    console.log('login called with name:', name);
    try {
      const res = await axios.post("/api/login", { name, password });
      console.log('Login response:', res);
      saveAccessToken(res.data);
    } catch (error) {
      console.log('Error during login:', error);
      throw error;
    }
  },

  getPublic: async () => {
    console.log('getPublic called.');
    try {
      const res = await axios.get("/api/Public");
      console.log('getPublic response:', res);
      return res.data;
    } catch (error) {
      console.log('Error fetching public data:', error);
      throw error;
    }
  },

  getPrivate: async () => {
    console.log('getPrivate called.');
    try {
      const res = await axios.get("/api/Private");
      console.log('getPrivate response:', res);
      return res.data;
    } catch (error) {
      console.log('Error fetching private data:', error);
      throw error;
    }
  },
};

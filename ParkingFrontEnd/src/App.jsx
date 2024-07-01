import React from 'react';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import 'react-toastify/dist/ReactToastify.css';
import { ToastContainer, toast } from 'react-toastify';
import {
  About,
  Error,
  History, //f
  NotFound,
  Payment, //f
  Settings,
  SignIn,
  SignUpAdmin,
  SignUpDriver,
  Profile, //f
  Timer, //f
  Unauthorized,
} from './pages';

import { Root, RootSign, RootPayment } from './components';
import { createDriver } from './utils/forms';
import { ThemeProvider } from 'styled-components';
import sharedVariables from './styles/sharedVariables';
const router = createBrowserRouter([
  {
    path: '/',
    element: <Root />,
    errorElement: <Error />,
    children: [
      {
        path: 'about',
        element: <About />,
      },
      {
        path: 'history',
        element: <History />,
      },
      {
        path: 'settings',
        element: <Settings />,
      },
      {
        path: 'timer',
        element: <Timer />,
      },
    ],
  },
  {
    path: '/profile',
    element: <Profile />,
  },
  {
    path: '/',
    element: <RootPayment />,
    errorElement: <Error />,
    children: [
      {
        path: 'payment',
        element: <Payment />,
      },
    ],
  },
  {
    path: '/',
    element: <RootSign />,

    children: [
      {
        path: '/signUp',
        // action: createDriver,
        element: <SignUpDriver />,
      },
      {
        path: '/signUpAdmin',
        element: <SignUpAdmin />,
      },
      {
        path: '/signIn',
        element: <SignIn />,
      },
    ],
  },
]);
function App() {
  return (
    <ThemeProvider theme={sharedVariables}>
      <ToastContainer position="top-center" autoClose={2000} />
      <RouterProvider router={router}></RouterProvider>
    </ThemeProvider>
  );
}

export default App;

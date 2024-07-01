import { redirect } from 'react-router-dom';
import {
  validateFormDriver,
  validateFormParkingAdmin,
  validateFormAddCard,
  validateFormSignIn,
} from './validateFields';
import axios from 'axios';
import { BASE_URL } from '../utils/consts';

// export async function createDriver({ request }) {
//   const data = Object.fromEntries(await request.formData());
//   console.log(data.email);
//   console.log('Action in signup driver page forms.js');
//   console.log('End');
//   return redirect('/timer');
// }

export const createDriver = async (formData) => {
  validateFormDriver(formData);
  try {
    const response = await axios({
      method: 'post',
      url: `${BASE_URL}/api/account/register`,
      data: { ...formData },
    });
    const data = response.data;
    if (!data.token) {
      throw new Error('Something went wrong, please try again later');
    }
    localStorage.setItem('access_token', data.token);
    window.location.href = '/profile';
  } catch (error) {
    if (error.response.status === 409) {
      throw new Error('User with provided data already exists. Try to log in');
    }
    console.log(error.message);
  }
  // } catch (error) {
  //   throw new Error(error.message);
  // }
  // console.log(1);
  // await new Promise((resolve) => setTimeout(resolve, 1000));
  // console.log(2);
  // console.log(formData);
  // navigate('/timer');
};

export const signInDriver = async (formData) => {
  validateFormSignIn(formData);
  try {
    const response = await axios({
      method: 'post',
      url: `${BASE_URL}/api/account/login`,
      data: { ...formData },
    });
    localStorage.setItem('access_token', response.data.token);
    window.location.href = '/profile';
  } catch (error) {
    if (error.response.status === 401)
      throw new Error('Invalid email or password');
  }
};

export const createParkingAdmin = async (formData) => {
  if (validateFormParkingAdmin(formData)) {
    window.location.href = '/profile';
  }
};

export const addCard = async (formData) => {
  validateFormAddCard(formData);
};

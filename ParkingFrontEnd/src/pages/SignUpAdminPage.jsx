import React from 'react';
import styled from 'styled-components';
import { toast } from 'react-toastify';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { createParkingAdmin } from '../utils/forms';

function SignUpAdminPage() {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    password: '',
    email: '',
    first_name: '',
    last_name: '',
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  async function handleSubmit(e) {
    e.preventDefault();
    try {
      await createParkingAdmin(formData);
    } catch (error) {
      toast.error(error.message);
      return;
    }
    toast.success('CREATED ADMIN');
    navigate('/profile');
  }

  return (
    <Wrapper>
      <form
        onSubmit={(e) => {
          handleSubmit(e, formData, navigate);
        }}
      >
        <h2>Sign Up As Parking Admin</h2>
        <div className="form-group">
          <label htmlFor="inputEmail">Email</label>
          <input
            type="email"
            className="form-control"
            id="inputEmail"
            name="email"
            aria-describedby="emailHelp"
            placeholder="cool_admin@gmail.com"
            value={formData.email}
            onChange={handleChange}
          />
        </div>
        <div className="form-group">
          <label htmlFor="first_name">First Name</label>
          <input
            type="text"
            className="form-control"
            id="first_name"
            name="first_name"
            placeholder="John"
            value={formData.first_name}
            onChange={handleChange}
          />
        </div>
        <div className="form-group">
          <label htmlFor="last_name">Last Name</label>
          <input
            type="text"
            className="form-control"
            id="last_name"
            name="last_name"
            placeholder="John"
            value={formData.last_name}
            onChange={handleChange}
          />
        </div>
        <div className="form-group">
          <label htmlFor="inputPassword">Password</label>
          <input
            type="password"
            name="password"
            className="form-control"
            id="inputPassword"
            placeholder="not qwerty please"
            value={formData.password}
            onChange={handleChange}
          />
        </div>
        <button type="submit" className="btn btn-primary btn-block">
          Sign Up!
        </button>
      </form>
    </Wrapper>
  );
}

const Wrapper = styled.main`
  display: block;
`;

export default SignUpAdminPage;

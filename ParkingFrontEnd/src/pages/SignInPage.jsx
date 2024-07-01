import React from 'react';
import styled from 'styled-components';
import { toast } from 'react-toastify';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { signInDriver } from '../utils/forms';

function SignInPage() {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    RegistrationType: 'driver',
    Password: '',
    Email: '',
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
      await signInDriver(formData);
    } catch (error) {
      toast.error(error.message);
      return;
    }
    toast.success('Successfully logged in');
  }
  return (
    <Wrapper>
      <form
        onSubmit={(e) => {
          handleSubmit(e, formData, navigate);
        }}
      >
        <h2>Sign In As Driver</h2>

        <div className="form-group">
          <label htmlFor="Email">Email</label>
          <input
            type="Email"
            className="form-control"
            id="Email"
            name="Email"
            aria-describedby="emailHelp"
            placeholder="CoolDriver@mail.com"
            value={formData.Email}
            onChange={handleChange}
          />
        </div>
        <div className="form-group">
          <label htmlFor="Password">Password</label>
          <input
            type="password"
            name="Password"
            className="form-control"
            id="Password"
            placeholder="Strong Password"
            value={formData.Password}
            onChange={handleChange}
          />
        </div>
        <button type="submit" className="btn btn-primary btn-block">
          Sign In!
        </button>
      </form>
    </Wrapper>
  );
}

const Wrapper = styled.main`
  display: block;
`;

export default SignInPage;

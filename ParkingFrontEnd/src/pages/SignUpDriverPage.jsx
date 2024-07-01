import styled from 'styled-components';
import { toast } from 'react-toastify';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { createDriver } from '../utils/forms';

function SignUpDriverPage() {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    RegistrationType: 'driver',
    FirstName: '',
    LastName: '',
    Password: '',
    Email: '',
    LicensePlateNumber: '',
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
      await createDriver(formData);
    } catch (error) {
      toast.error(error.message);
      return;
    }
    toast.success('CREATED DRIVER');
  }
  return (
    <Wrapper>
      <form
        onSubmit={(e) => {
          handleSubmit(e, formData, navigate);
        }}
      >
        <h2>Sign Up As Driver</h2>
        <div className="form-group">
          <label htmlFor="FirstName">First Name</label>
          <input
            type="text"
            className="form-control"
            id="FirstName"
            name="FirstName"
            placeholder="John"
            value={formData.FirstName}
            onChange={handleChange}
          />
        </div>
        <div className="form-group">
          <label htmlFor="LastName">Last Name</label>
          <input
            type="text"
            className="form-control"
            id="LastName"
            name="LastName"
            placeholder="Doe"
            value={formData.LastName}
            onChange={handleChange}
          />
        </div>
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
        <div className="form-group">
          <label htmlFor="LicensePlateNumber">Car License Plate Number</label>
          <input
            type="text"
            name="LicensePlateNumber"
            className="form-control"
            id="LicensePlateNumber"
            placeholder="WX1234A"
            value={formData.LicensePlateNumber}
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

export default SignUpDriverPage;

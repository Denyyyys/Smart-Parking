import React from 'react';
import styled from 'styled-components';
import { toast } from 'react-toastify';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { addCard } from '../utils/forms';

function PaymentPage() {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    cardNumber: '',
    date: '',
    cvv: '',
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
      await addCard(formData);
    } catch (error) {
      toast.error(error.message);
      return;
    }
    toast.success('CARD ADDED TO YOUR ACCOUNT');
  }
  return (
    <Wrapper>
      <form
        onSubmit={(e) => {
          handleSubmit(e, formData, navigate);
        }}
      >
        <h2>Add Your Card</h2>
        <div className="form-group">
          <label htmlFor="inputCardNumber">Enter Card Number</label>
          <input
            type="number"
            className="form-control"
            id="inputCardNumber"
            name="cardNumber"
            aria-describedby="cardNumberHelp"
            placeholder="1234123412341234"
            required
            maxlength="16"
            value={formData.cardNumber}
            onChange={handleChange}
          />
        </div>
        <div className="form-group">
          <label htmlFor="inputDate">Enter Experation Date</label>
          <input
            type="month"
            name="date"
            className="form-control"
            id="inputDate"
            min="2024-05"
            value={formData.date}
            onChange={handleChange}
          />
        </div>
        <div className="form-group">
          <label htmlFor="inputCvv">CVV</label>
          <input
            type="password"
            name="cvv"
            className="form-control"
            id="inputCvv"
            placeholder="CVV"
            required
            maxlength="3"
            value={formData.cvv}
            onChange={handleChange}
          />
        </div>
        <button type="submit" className="btn btn-primary btn-block">
          Add card
        </button>
      </form>
    </Wrapper>
  );
}

const Wrapper = styled.main`
  display: block;
`;

export default PaymentPage;

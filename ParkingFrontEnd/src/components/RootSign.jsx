import React from 'react';
import { Outlet } from 'react-router-dom';
import { FaCarAlt } from 'react-icons/fa';
import styled from 'styled-components';

export async function submitForm() {
  console.log('form is submitted!');
  return null;
}
function RootSign() {
  return (
    <Wrapper>
      <section className="container">
        <div className="sign-header">
          <FaCarAlt className="car-icon" />
          <h4>Smart Parking</h4>
        </div>
        <Outlet />
      </section>
    </Wrapper>
  );
}

const Wrapper = styled.section`
  display: grid;
  place-items: center;
  align-items: center;
  min-width: calc(100vw - 40px);
  min-height: calc(100vh - 10px - 30px);
  margin: 10px 250px 10px 20px;
  .car-icon {
    width: 80vw;
    max-width: 200px;
    height: 80vw;
    max-height: 200px;
    display: block;
    margin: auto;
    color: ${(props) => props.theme.clrPrimary800};
  }
  .container {
    min-width: 100%;
    min-height: 100%;
    display: grid;
    grid-template-rows: auto 1fr;
    gap: 10px;
    align-items: center;
  }
  .sign-header {
    text-align: center;
  }
  p {
    color: red;
  }
  .form-group {
    margin-bottom: 15px;
  }
`;

export default RootSign;

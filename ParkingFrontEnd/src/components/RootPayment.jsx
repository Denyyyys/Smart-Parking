import React from 'react';
import { Outlet, Link } from 'react-router-dom';
import { CiCreditCard1 } from 'react-icons/ci';
import { TiArrowBack } from 'react-icons/ti';
import styled from 'styled-components';

export async function submitForm() {
  console.log('form is submitted!');
  return null;
}

function RootPayment() {
  return (
    <Wrapper>
      <section className="upBar">
        <div>
          <Link to="/profile">
            <TiArrowBack className="back-icon" />
          </Link>
        </div>
        <div className="menu-name">
          <h4>Payment</h4>
        </div>
      </section>
      <hr width="100%" align="center"></hr>
      <section className="container">
        <div className="sign-header">
          <CiCreditCard1 className="card-icon" />
        </div>
        <Outlet />
      </section>
    </Wrapper>
  );
}

const Wrapper = styled.section`
  /* display: grid; */
  /* place-items: center; */
  /* align-items: center; */
  min-width: calc(100vw - 40px);
  min-height: calc(100vh - 10px - 30px);
  margin: 10px 250px 10px 20px;
  .card-icon {
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
  .menu-name {
    text-align: right;
    margin-top: 10%;
  }
  .form-group {
    margin-bottom: 15px;
  }
  .upBar {
    display: flex;
    justify-content: space-between;
    align-items: center;
    gap: 10px;
    margin-bottom: 10px; /* dodałem margines dolny */
  }
  .back-icon {
    width: 10vw;
    max-width: 100px;
    height: 10vw;
    max-height: 100px;
    /* display: block; */
    /* margin: auto; */
    color: ${(props) => props.theme.clrPrimary800};
    align-items: left;
  }
`;

export default RootPayment;

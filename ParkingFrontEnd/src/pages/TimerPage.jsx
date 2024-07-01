import React, { useState, useEffect } from 'react';
import { Outlet, Link } from 'react-router-dom';
import { RxLapTimer } from 'react-icons/rx';
import { TiArrowBack } from 'react-icons/ti';
import styled from 'styled-components';
import ProgressBar from '../utils/progressBar';
import axios from 'axios';
import { toast } from 'react-toastify';
import TimeFormatter from '../utils/TimeFormatter';
import ParkingTime from '../utils/clock';
import { BASE_URL } from '../utils/consts';

export async function submitForm() {
  console.log('form is submitted!');
  return null;
}

function RootPayment() {
  // const [start, setStart] = useState(false);

  // const handleStart = () => {
  //   setStart(true);
  // };
  // const [time, setTime] = useState([]);
  // const [loading, setLoading] = useState(true);
  // const [error, setError] = useState(null);

  // useEffect(() => {
  //   const fetchTime = async () => {
  //     // Tu dodaj kod do pobierania danych z bazy danych
  //     try {
  //       // const response = await axios(`${BASE_URL}/api/Parking/18`);
  //       // localStorage.setItem('access_token', JSON.stringify(response.data));
  //       const token = localStorage.getItem('access_token');
  //       const response = await axios.get(
  //         `${BASE_URL}/api/user/timeleft`,
  //         {
  //           headers: {
  //             Authorization: `Bearer ${token}`,
  //           },
  //         }
  //       );
  //       setTime(response.data);
  //       console.log(time);
  //       if (Number.isInteger(time)){
  //         console.log('w ifie');
  //         handleStart();}

  //     } catch (error) {
  //       if (error.response.status === 401) {
  //         navigate('signin');
  //         toast.error('You are not logged in');
  //         return;
  //       }
  //       setError(error);
  //     } finally {
  //       setLoading(false);
  //     }
  //   };

  //   fetchTime();
  // }, []);

  // console.log(time);
  // if (Number.isInteger(time)){
  //   console.log('w ifie');
  //   handleStart();}

  return (
    <Wrapper>
      <section className="upBar">
        <div>
          <Link to="/profile">
            <TiArrowBack className="back-icon" />
          </Link>
        </div>
        <div className="menu-name">
          <h4>Time Left</h4>
        </div>
      </section>
      <hr width="100%" align="center"></hr>
      <section className="container">
        <div className="sign-header">
          <RxLapTimer className="card-icon" />
        </div>
        <Outlet />
      </section>
      <section className="container">
        {/* <ProgressBar totalTime={1200} initialTime={0} start={start}/> */}
        {/* <TimeFormatter seconds={time} /> */}
        <ParkingTime></ParkingTime>
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

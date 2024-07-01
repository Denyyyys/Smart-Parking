import React, { useState, useEffect } from 'react';
import { Outlet, Link, useNavigate } from 'react-router-dom';
import { CiCreditCard1 } from 'react-icons/ci';
import { IoMdSettings } from 'react-icons/io';
import { MdHistoryEdu } from 'react-icons/md';
import { RxLapTimer } from 'react-icons/rx';
import PayModal from '../components/PayModal';
import ProgressBar from '../utils/progressBar';
import ParkingTime from '../utils/clock';
import styled from 'styled-components';
import axios from 'axios';
import { toast } from 'react-toastify';
import { BASE_URL } from '../utils/consts';
export async function submitForm() {
  console.log('form is submitted!');
  return null;
}

function ProfilePage() {
  const [start, setStart] = useState(false);

  const handleStart = () => {
    setStart(true);
  };

  const navigate = useNavigate();
  const [parkingData, setparkingData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [modalIsOpen, setModalIsOpen] = useState(false);
  const [isParked, setIsParked] = useState(false);

  useEffect(() => {
    const fetchparkingData = async () => {
      try {
        // const response = await axios(`${BASE_URL}/api/Parking/18`);
        // localStorage.setItem('access_token', JSON.stringify(response.data));
        const token = localStorage.getItem('access_token');
        const response = await axios.get(`${BASE_URL}/api/user/stats`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        setparkingData(response.data);
      } catch (error) {
        if (error.response.status === 401) {
          navigate('signin');
          toast.error('You are not logged in');
          return;
        }
        setError(error);
      } finally {
        setLoading(false);
      }
    };

    fetchparkingData();
  }, []);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error.message}</div>;
  }
  console.log(parkingData);
  return (
    <Wrapper className="profileWrapper">
      {modalIsOpen && (
        <PayModal modalIsOpen={modalIsOpen} setModalIsOpen={setModalIsOpen} />
      )}

      {/* <section className="upBar">
        <div className="menu-name">
          <h4>Main Page</h4>
        </div>
      </section> 
       <hr width="100%" align="center"></hr> */}
      <section className="container">
        {/* <div> */}
        {/* <progress value={75} max={100} /> */}
        {/* <ProgressBar totalTime={1200} initialTime={0} start={start}/> */}
        {/* </div> */}
        <ParkingTime
          isParked={isParked}
          setIsParked={setIsParked}
          setModalIsOpen={setModalIsOpen}
          modalIsOpen={modalIsOpen}
        ></ParkingTime>

        <Outlet />
      </section>
      <hr width="100%" align="center"></hr>
      <section className="menu">
        <div className="link">
          <a href="/timer">
            <div className="rectangle">
              <RxLapTimer className="timer-icon" />
              <span>Time Left</span>
            </div>
          </a>
        </div>
        <div className="link">
          <a href="/payment">
            <div className="rectangle">
              <CiCreditCard1 className="card-icon" />
              <span>Payment</span>
            </div>
          </a>
        </div>

        <div className="link">
          <a href="/history">
            <div className="rectangle">
              <MdHistoryEdu className="history-icon" />
              <span>History</span>
            </div>
          </a>
        </div>
        <div className="link">
          <a href="/settings">
            <div className="rectangle">
              <IoMdSettings className="settings-icon" />
              <span>Settings</span>
            </div>
          </a>
        </div>
      </section>
      <hr width="100%" align="center"></hr>
      <section>
        <div>
          <div className="rectangle_pro">
            <label>First name: {parkingData.firstName}</label>
          </div>
          <div className="rectangle_pro">
            <label>Last name: {parkingData.lastName}</label>
          </div>
          <div className="rectangle_pro">
            <label>Email: {parkingData.email}</label>
          </div>
          <div className="rectangle_pro">
            <label>License plate: {parkingData.licensePlateNumber}</label>
          </div>
        </div>
      </section>
    </Wrapper>
  );
}

const Wrapper = styled.section`
  a {
    color: inherit;
    text-decoration: none;
  }
  .container {
    align-items: center;
  }
  .menu-name {
    text-align: right;
    margin-top: 10%;
    margin-right: 2%;
  }
  .menu {
    display: flex;
    flex-direction: column;
    align-items: center;
    margin-top: 3%;
  }

  .rectangle {
    display: flex;
    min-width: 100%;
    align-items: center;
    text-align: center;
    padding: 10px 20px;
    border: 1px solid #00627a;
    background-color: #4dbfdb;
    border-radius: 5px;
    cursor: pointer;
  }
  .rectangle_pro {
    display: flex;
    min-width: 100%;
    align-items: center;
    text-align: center;
    padding: 10px 20px;
    border: 1px solid #2793af;
    background-color: #7ed5ea;
    border-radius: 5px;
    cursor: pointer;
  }
  .timer-icon {
    width: 5vw;
    max-width: 30px;
    height: 5vw;
    max-height: 30px;
    display: block;
    color: black;
    margin-right: 2%;
  }
  .card-icon {
    width: 5vw;
    max-width: 30px;
    height: 5vw;
    max-height: 30px;
    display: block;
    color: black;
    margin-right: 2%;
  }
  .history-icon {
    width: 5vw;
    max-width: 30px;
    height: 5vw;
    max-height: 30px;
    display: block;
    color: black;
    margin-right: 2%;
  }
  .settings-icon {
    width: 5vw;
    max-width: 30px;
    height: 5vw;
    max-height: 30px;
    display: block;
    color: black;
    margin-right: 2%;
  }
  .link {
    min-width: 100%;
  }
  .progress-container {
    width: 100%;
    background-color: #f3f3f3;
    border: 1px solid #ccc;
    border-radius: 5px;
    margin: 20px 0;
    height: 30px;
  }

  .progress-bar {
    height: 100%;
    width: 0;
    background-color: #4caf50;
    text-align: center;
    line-height: 30px;
    color: white;
    border-radius: 5px;
  }
`;

export default ProfilePage;

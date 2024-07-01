import React, { useState, useEffect } from 'react';
import { Outlet, Link } from 'react-router-dom';
import { MdHistoryEdu } from 'react-icons/md';
import { TiArrowBack } from 'react-icons/ti';
import styled from 'styled-components';
import axios from 'axios';
import { toast } from 'react-toastify';
import TimeFormatter from '../utils/TimeFormatter';
import { BASE_URL } from '../utils/consts';

export async function submitForm() {
  console.log('form is submitted!');
  return null;
}

function RootPayment() {
  const [history, setHistory] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    // Symulowane pobieranie danych z bazy danych
    const fetchHistory = async () => {
      // Tu dodaj kod do pobierania danych z bazy danych
      try {
        // const response = await axios(`${BASE_URL}/api/Parking/18`);
        // localStorage.setItem('access_token', JSON.stringify(response.data));
        const token = localStorage.getItem('access_token');
        const response = await axios.get(`${BASE_URL}/api/user/history`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        setHistory(response.data);
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

    fetchHistory();
  }, []);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error.message}</div>;
  }
  console.log(history);

  function formatDate(date) {
    const formattedDate = date.slice(0, 16).replace('T', ' ');
    return formattedDate;
  }

  return (
    <Wrapper>
      <section className="upBar">
        <div>
          <Link to="/profile">
            <TiArrowBack className="back-icon" />
          </Link>
        </div>
        <div className="menu-name">
          <h4>History</h4>
        </div>
      </section>
      <hr width="100%" align="center"></hr>
      <section className="container">
        <div className="sign-header">
          <MdHistoryEdu className="card-icon" />
        </div>
        <Outlet />
      </section>
      <div className="history-container">
        {history.length === 0 ? (
          <p>Ładowanie historii...</p>
        ) : (
          <ul>
            {history.map((record, index) => (
              <li key={index}>
                <p>Parking: {record.parkingName}</p>
                <p>Started parking: {formatDate(record.entryDate)}</p>
                <p>Ended parking: {formatDate(record.exitDate)}</p>
                <p>Parking spot: {record.parkingPlace}</p>
              </li>
            ))}
          </ul>
        )}
      </div>
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
  .history-container {
    width: 80%;
    margin: 0 auto;
    text-align: left;
  }

  h1 {
    text-align: center;
    margin-bottom: 20px;
  }

  ul {
    list-style-type: none;
    padding: 0;
  }

  li {
    background-color: #f9f9f9;
    margin-bottom: 10px;
    padding: 15px;
    border-radius: 5px;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  }

  li p {
    margin: 5px 0;
  }
`;

export default RootPayment;

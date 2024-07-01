import React, { useState, useEffect } from 'react';
import TimeFormatter from './TimeFormatter';
// import '../utils/ProgressBar.css';
import { BASE_URL } from '../utils/consts';

function ProgressBar({ totalTime, initialTime, start }) {
  // const [time, setTime] = useState([]);
  // const [loading, setLoading] = useState(true);
  // const [error, setError] = useState(null);

  // useEffect(() => {
  // Symulowane pobieranie danych z bazy danych
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
  const [currentTime, setCurrentTime] = useState(initialTime);

  useEffect(() => {
    // if (!start) return;
    const interval = setInterval(() => {
      setCurrentTime((prevTime) => {
        if (prevTime < totalTime) {
          return prevTime + 1;
        } else {
          clearInterval(interval);
          return prevTime;
        }
      });
    }, 1000);

    return () => clearInterval(interval);
  }, [totalTime]);

  // if (loading) {
  //   return <div>Loading...</div>;
  // }

  // if (error) {
  //   return <div>Error: {error.message}</div>;
  // }

  const progress = ((totalTime - currentTime) / totalTime) * 100;

  const containerStyle = {
    width: '100%',
    backgroundColor: '#f3f3f3',
    border: '1px solid #ccc',
    borderRadius: '5px',
    margin: '20px 0',
    height: '30px',
  };

  const progressBarStyle = {
    height: '100%',
    width: `${progress}%`,
    backgroundColor: '#4caf50',
    textAlign: 'center',
    lineHeight: '30px',
    color: 'white',
    borderRadius: '5px',
    transition: 'width 0.5s ease',
  };

  return (
    <div style={containerStyle}>
      <div style={progressBarStyle}>
        {currentTime >= totalTime ? (
          'Koniec!'
        ) : (
          <TimeFormatter seconds={totalTime - currentTime} />
        )}
        {/* {currentTime >= totalTime ? 'Koniec!' : <TimeFormatter seconds={time} />} */}
      </div>
    </div>
  );
}

// const Wrapper = styled.section`
//   width: 100%;
//   height: 30px;
//   background-color: #ccedf5;
//   align-items: center;
//   border-radius: 5px;
//   margin-top: 20px;

//   .progress-container {
//   width: 100%;
//   background-color: #f3f3f3;
//   border: 1px solid #ccc;
//   border-radius: 5px;
//   margin: 20px 0;
//   height: 30px;
// }

// .progress-bar {
//   height: 100%;
//   width: 0;
//   background-color: #4caf50;
//   text-align: center;
//   line-height: 30px;
//   color: white;
//   border-radius: 5px;
//   transition: width 0.5s ease;
// }
// `;

export default ProgressBar;

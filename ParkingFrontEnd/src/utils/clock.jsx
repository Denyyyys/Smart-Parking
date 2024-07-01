import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { BASE_URL } from '../utils/consts';

function ParkingTime({ isParked, setIsParked, setModalIsOpen, modalIsOpen }) {
  const [timeLeft, setTimeLeft] = useState('');
  const [timeLeftSeconds, setTimeLeftSeconds] = useState(null);
  const [loading, setLoading] = useState(true);
  const [isParkedHere, setIsParkedHere] = useState(false);
  const [flashPlace, setFlashPlace] = useState(false);

  async function flashLight() {
    setFlashPlace(true);
    let access_token = localStorage.getItem('access_token');
    const response = await axios.get(`${BASE_URL}/api/user/startFlash`, {
      headers: {
        Authorization: `Bearer ${access_token}`,
      },
    });
    console.log(response.data);
  }
  async function stopFlashingLight() {
    setFlashPlace(false);
    let access_token = localStorage.getItem('access_token');
    const response = await axios.get(`${BASE_URL}/api/user/stopflash`, {
      headers: {
        Authorization: `Bearer ${access_token}`,
      },
    });
    console.log(response.data);
  }
  let a = false;
  useEffect(() => {
    // Funkcja do pobierania danych z serwera
    const fetchParkingTime = async () => {
      try {
        const token = localStorage.getItem('access_token');
        const response = await axios.get(`${BASE_URL}/api/user/timeleft`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        // console.log(response);
        // console.log(typeof response.data);
        if (typeof response.data === 'string') {
          setTimeLeft(response.data);
          setTimeLeftSeconds(null);
        } else if (typeof response.data === 'number') {
          a = true;
          setModalIsOpen(false);
          setIsParkedHere(true);
          setTimeLeft(formatTime(response.data));
          setTimeLeftSeconds(response.data);
        }
      } catch (error) {
        console.error('Błąd podczas pobierania danych:', error);
        setTimeLeft('Błąd podczas pobierania danych');
        setTimeLeftSeconds(null);
      } finally {
        setLoading(false);
      }
    };

    fetchParkingTime();

    const interval = setInterval(() => {
      setTimeLeftSeconds((prev) => {
        if (prev !== null) {
          return prev - 1;
        }
        return prev;
      });
    }, 1000);

    return () => clearInterval(interval);
  }, []);

  useEffect(() => {
    const fetchParkingTime = async () => {
      try {
        const token = localStorage.getItem('access_token');
        const response = await axios.get(`${BASE_URL}/api/user/timeleft`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        console.log(`isParked: ${isParked}`);
        console.log(`modalIsOpen: ${modalIsOpen}`);
        console.log(`responsedata: ${response.data}`);
        if (typeof response.data === 'string') {
          console.log(response.data);
          if (isParkedHere) {
            console.log(12412423142);
            setModalIsOpen((prevModal) => true);
          }
          if (a) {
            setModalIsOpen(true);
            setModalIsOpen((prevModal) => true);
            a = false;
          }
          setIsParked((p) => false);
          setIsParkedHere((ph) => false);

          setTimeLeft(response.data);
          setTimeLeftSeconds((t) => null);
        } else if (typeof response.data === 'number') {
          a = true;
          setModalIsOpen((p) => false);
          setIsParkedHere((p) => true);
          console.log(`isParked2: ${isParked}`);
          setIsParked((p) => true);
          setTimeLeft((p) => formatTime(response.data));
          setTimeLeftSeconds((d) => response.data);
        }
      } catch (error) {
        console.error('Błąd podczas pobierania danych:', error);
        setTimeLeft('Błąd podczas pobierania danych');
        setTimeLeftSeconds(null);
      } finally {
        setLoading(false);
      }
    };

    const intervalId = setInterval(fetchParkingTime, 5000);

    return () => clearInterval(intervalId);
  }, []);

  useEffect(() => {
    if (timeLeftSeconds !== null) {
      setTimeLeft(formatTime(timeLeftSeconds));
    }
  }, [timeLeftSeconds]);

  // Funkcja do formatowania czasu w sekundach na xxh:xxmin:xxs
  const formatTime = (seconds) => {
    const hours = Math.floor(Math.abs(seconds) / 3600);
    const minutes = Math.floor((Math.abs(seconds) % 3600) / 60);
    const secs = Math.abs(seconds) % 60;

    return `${hours}h:${minutes.toString().padStart(2, '0')}min:${secs
      .toString()
      .padStart(2, '0')}s`;
  };

  if (loading) {
    return <div>Ładowanie...</div>;
  }

  let progress = (timeLeftSeconds / 1200) * 100;
  if (progress < 0) {
    progress = 100;
  }

  const containerStyle = {
    width: '100%',
    backgroundColor: '#f3f3f3',
    border: timeLeftSeconds != null ? '1px solid #ccc' : '0px',
    borderRadius: '5px',
    margin: timeLeftSeconds != null ? '20px 0' : '0',
    height: timeLeftSeconds != null ? '30px' : '0',
  };

  const progressBarStyle = {
    height: timeLeftSeconds != null ? '100%' : '0',
    width: `${progress}%`,
    backgroundColor: timeLeftSeconds > 0 ? '#4caf50' : '#b62a18',
    textAlign: 'center',
    lineHeight: '30px',
    color: 'white',
    borderRadius: '5px',
    transition: 'width 0.5s ease',
  };
  return (
    <div style={styles.container}>
      <h1 style={styles.title}>
        {timeLeftSeconds != null
          ? timeLeftSeconds > 0
            ? 'Pozostały czas parkowania'
            : 'Opóźnienie w parkowaniu wynosi'
          : ''}
      </h1>
      <p style={styles.time}>{timeLeft}</p>
      <div style={containerStyle}>
        <div style={progressBarStyle}></div>
      </div>
      {timeLeftSeconds != null && !flashPlace && (
        <button className="btn btn-primary btn-block" onClick={flashLight}>
          Flash My Place
        </button>
      )}
      {timeLeftSeconds != null && flashPlace && (
        <button
          className="btn btn-primary btn-block"
          onClick={stopFlashingLight}
        >
          Stop Flashing My Place
        </button>
      )}
    </div>
  );
}
const styles = {
  container: {
    textAlign: 'center',
    marginTop: '50px',
  },
  title: {
    fontSize: '24px',
    marginBottom: '20px',
  },
  time: {
    fontSize: '48px',
    fontWeight: 'bold',
  },
  loading: {
    textAlign: 'center',
    fontSize: '24px',
  },
};

export default ParkingTime;

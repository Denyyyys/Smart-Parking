import React, { useState } from 'react';
import qr_img from '../assets/qr_code.png';
import styled from 'styled-components';
import { IoMdClose } from 'react-icons/io';
function PayModal({ setModalIsOpen, modalIsOpen }) {
  const [toPay, setToPay] = useState(false);

  function closemodal() {
    setModalIsOpen(false);
  }
  return (
    <Wrapper>
      <ModalContent>
        <div className="">
          <IoMdClose className="close-btn" onClick={closemodal} />
        </div>
        {toPay ? (
          <img src={qr_img} alt="QR code" className="qr-img" />
        ) : (
          <button
            className="btn btn-success btn-block pay-btn"
            onClick={() => {
              setToPay(true);
            }}
          >
            Pay
          </button>
        )}
        {toPay && <h2>Scan the code to leave</h2>}
      </ModalContent>
    </Wrapper>
  );
}

const Wrapper = styled.div`
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.6); /* Semi-transparent background */
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000; /* Ensures the modal is on top of other elements */
`;

const ModalContent = styled.div`
  position: relative;
  padding-top: 100px !important;
  background: #fff; /* White background for the modal content */
  padding: 20px;
  border-radius: 8px; /* Rounded corners */
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2); /* Subtle shadow for depth */
  text-align: center;
  h2 {
    margin-top: 20px;
  }
  .qr-img {
    width: 70%;
    max-width: 300px; /* Maximum width of the image */
    height: auto; /* Maintain aspect ratio */
    border: 5px solid #333; /* Add a border to the image */
    padding: 10px; /* Add some padding around the image */
    background-color: #fff; /* White background for the image */
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2); /* Add a subtle shadow */
  }
  .close-btn {
    position: absolute;
    top: 10px;
    right: 10px;
    width: 50px;
    max-width: 100px;
    height: 50px;
    max-height: 100px;
    /* display: block; */
    /* margin: auto; */
    color: red;
    align-items: left;
  }
  .pay-btn {
    font-size: 30px;
    min-width: 60vw;
  }
`;

export default PayModal;

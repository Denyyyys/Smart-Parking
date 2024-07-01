export const validateFormDriver = (formData) => {
  for (const field in formData) {
    if (!formData[field]) {
      throw new Error('Please fill in all fields.');
    }
  }
  if (formData.RegistrationType !== 'driver') {
    throw new Error('Registration type must be "driver"');
  }

  let LicencePlateNumberRegex = /^[A-Za-z0-9]+$/;
  if (!formData.LicensePlateNumber.match(LicencePlateNumberRegex)) {
    throw new Error(
      'License Plate Number must contain only letters and numbers!'
    );
  }

  return true;
};

export const validateFormParkingAdmin = (formData) => {
  for (const field in formData) {
    if (!formData[field]) {
      throw new Error('Please fill in all fields.');
    }
  }
  return true;
};

export const validateFormAddCard = (formData) => {
  for (const field in formData) {
    if (!formData[field]) {
      throw new Error('Please fill in all fields.');
    }
  }
  return true;
};

export const validateFormSignIn = (formData) => {
  for (const field in formData) {
    if (!formData[field]) {
      throw new Error('Please fill in all fields.');
    }
  }
};

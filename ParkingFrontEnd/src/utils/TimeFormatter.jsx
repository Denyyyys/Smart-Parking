import React from 'react';

const TimeFormatter = ({ seconds }) => {
  const formatTime = (seconds) => {
    const hrs = Math.floor(seconds / 3600);
    const mins = Math.floor((seconds % 3600) / 60);
    const secs = seconds % 60;

    return `Pozostało ${hrs}h ${mins}min ${secs}s`;
  };

  return (
    <div>
      {formatTime(seconds)}
    </div>
  );
};

export default TimeFormatter;

import React from 'react'

function ThankYou() {
  return (
    <div className="card w-full max-w-md text-center">
      <div className="mb-6">
        <div className="w-16 h-16 bg-green-100 rounded-full flex items-center justify-center mx-auto mb-4">
          <svg className="w-8 h-8 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
          </svg>
        </div>
        <h2 className="text-3xl font-bold text-gray-900 mb-2">Thank You!</h2>
        <p className="text-gray-600 text-lg">You will get an email shortly</p>
      </div>
      
      <div className="bg-gray-50 rounded-lg p-4 mb-6">
        <p className="text-sm text-gray-600">
          We've sent a confirmation email with your account details. 
          Please check your inbox and follow the instructions to complete your account setup.
        </p>
      </div>
      
      <button
        onClick={() => window.location.reload()}
        className="btn-primary"
      >
        Start Over
      </button>
    </div>
  )
}

export default ThankYou 
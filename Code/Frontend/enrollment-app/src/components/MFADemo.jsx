import React, { useState } from 'react'
import MFASetup from './MFASetup'
import MFAComplete from './MFAComplete'

function MFADemo() {
  const [currentStep, setCurrentStep] = useState('setup') // 'setup' or 'complete'
  const [mfaResult, setMfaResult] = useState(null)

  // Mock user data from enrollment
  const mockUserData = {
    email: 'john.doe@example.com',
    phoneNumber: '+1 (555) 123-4567',
    username: 'johndoe',
    accountNumber: '1234567890'
  }

  const handleMFAComplete = (result) => {
    setMfaResult(result)
    setCurrentStep('complete')
  }

  const handleReset = () => {
    setCurrentStep('setup')
    setMfaResult(null)
  }

  if (currentStep === 'complete') {
    return <MFAComplete mfaResult={mfaResult} />
  }

  return (
    <div>
      <MFASetup userData={mockUserData} onComplete={handleMFAComplete} />
      
      {/* Demo controls */}
      <div className="fixed bottom-4 right-4 bg-white rounded-lg shadow-lg p-4 border">
        <h3 className="text-sm font-semibold text-gray-700 mb-2">Demo Controls</h3>
        <div className="space-y-2">
          <button
            onClick={handleReset}
            className="text-xs bg-gray-200 hover:bg-gray-300 text-gray-700 px-3 py-1 rounded"
          >
            Reset Demo
          </button>
          <div className="text-xs text-gray-500">
            <p>Email: {mockUserData.email}</p>
            <p>Phone: {mockUserData.phoneNumber}</p>
          </div>
        </div>
      </div>
    </div>
  )
}

export default MFADemo 
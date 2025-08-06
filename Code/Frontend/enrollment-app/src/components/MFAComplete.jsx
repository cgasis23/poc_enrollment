import React from 'react'

function MFAComplete({ mfaResult }) {
  const { mfaVerified, mfaMethod } = mfaResult

  return (
    <div className="min-h-screen bg-gradient-to-br from-gray-50 to-gray-100 py-8 px-4">
      <div className="max-w-4xl mx-auto">
        <div className="text-center mb-8">
          <h1 className="text-4xl font-bold text-gray-900 mb-2">Setup Complete!</h1>
          <p className="text-gray-600">Your account is now ready to use</p>
        </div>
        
        <div className="flex justify-center">
          <div className="card w-full max-w-md text-center">
            <div className="mb-6">
              <div className="w-16 h-16 bg-green-100 rounded-full flex items-center justify-center mx-auto mb-4">
                <svg className="w-8 h-8 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
                </svg>
              </div>
              <h2 className="text-3xl font-bold text-gray-900 mb-2">Account Secured!</h2>
              <p className="text-gray-600 text-lg">
                {mfaVerified 
                  ? `Two-factor authentication has been enabled via ${mfaMethod === 'email' ? 'email' : 'SMS'}`
                  : 'Two-factor authentication setup was skipped'
                }
              </p>
            </div>
            
            <div className="bg-gray-50 rounded-lg p-4 mb-6">
              <p className="text-sm text-gray-600">
                {mfaVerified ? (
                  <>
                    Your account is now protected with two-factor authentication. 
                    You'll need to enter a verification code in addition to your password when signing in.
                    <br /><br />
                    You can manage your security settings anytime from your account dashboard.
                  </>
                ) : (
                  <>
                    Your account has been created successfully. 
                    You can set up two-factor authentication later from your account settings for enhanced security.
                  </>
                )}
              </p>
            </div>
            
            <div className="space-y-3">
              <button
                onClick={() => window.location.href = '/dashboard'}
                className="w-full btn-primary"
              >
                Go to Dashboard
              </button>
              
              <button
                onClick={() => window.location.href = '/login'}
                className="w-full bg-gray-200 hover:bg-gray-300 text-gray-700 font-semibold py-3 px-6 rounded-lg transition-all duration-200"
              >
                Sign In
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}

export default MFAComplete 
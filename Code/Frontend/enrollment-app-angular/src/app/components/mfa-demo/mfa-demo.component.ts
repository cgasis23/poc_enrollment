import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MFASetupComponent } from '../mfa-setup/mfa-setup.component';
import { MFACompleteComponent } from '../mfa-complete/mfa-complete.component';

interface UserData {
  email: string;
  phoneNumber: string;
  username: string;
  accountNumber: string;
}

interface MFAResult {
  mfaVerified: boolean;
  mfaMethod: string | null;
}

@Component({
  selector: 'app-mfa-demo',
  standalone: true,
  imports: [CommonModule, MFASetupComponent, MFACompleteComponent],
  templateUrl: './mfa-demo.component.html',
  styleUrl: './mfa-demo.component.css'
})
export class MFADemoComponent {
  currentStep = signal<'setup' | 'complete'>('setup');
  mfaResult = signal<MFAResult | null>(null);

  // Mock user data from enrollment
  mockUserData: UserData = {
    email: 'john.doe@example.com',
    phoneNumber: '+1 (555) 123-4567',
    username: 'johndoe',
    accountNumber: '1234567890'
  };

  handleMFAComplete(result: MFAResult) {
    this.mfaResult.set(result);
    this.currentStep.set('complete');
  }

  handleReset() {
    this.currentStep.set('setup');
    this.mfaResult.set(null);
  }
}

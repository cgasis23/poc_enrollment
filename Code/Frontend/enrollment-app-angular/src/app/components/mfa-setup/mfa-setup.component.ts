import { Component, Input, Output, EventEmitter, signal, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MFAService } from '../../services/mfa.service';

interface UserData {
  email: string;
  phoneNumber: string;
  username: string;
  accountNumber: string;
}

interface MFAErrors {
  code?: string;
  general?: string;
}

interface MFAResult {
  mfaVerified: boolean;
  mfaMethod: string | null;
}

@Component({
  selector: 'app-mfa-setup',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './mfa-setup.component.html',
  styleUrl: './mfa-setup.component.css'
})
export class MFASetupComponent {
  @Input() userData!: UserData;
  @Output() onComplete = new EventEmitter<MFAResult>();

  mfaCode = signal('');
  mfaMethod = signal<'email' | 'sms'>('email');
  isCodeSent = signal(false);
  countdown = signal(0);
  errors = signal<MFAErrors>({});
  isLoading = signal(false);

  constructor(private mfaService: MFAService) {
    // Countdown timer effect
    effect(() => {
      if (this.countdown() > 0) {
        setTimeout(() => {
          this.countdown.update(prev => prev - 1);
        }, 1000);
      }
    });
  }

  // Send MFA code
  async sendMFACode() {
    this.isLoading.set(true);
    try {
      const identifier = this.mfaMethod() === 'email' ? this.userData.email : this.userData.phoneNumber;
      const result = this.mfaMethod() === 'email' 
        ? await this.mfaService.sendEmailCode(identifier)
        : await this.mfaService.sendSMSCode(identifier);
      
      if (result.success) {
        this.isCodeSent.set(true);
        this.countdown.set(30); // 30 second countdown
        this.errors.set({});
      } else {
        this.errors.set({ general: result.message });
      }
    } catch (error) {
      this.errors.set({ general: 'Failed to send code. Please try again.' });
    } finally {
      this.isLoading.set(false);
    }
  }

  handleInputChange(value: string) {
    // Only allow numbers and limit to 6 digits
    if (/^\d{0,6}$/.test(value)) {
      this.mfaCode.set(value);
      if (this.errors().code) {
        this.errors.update(prev => ({ ...prev, code: '' }));
      }
    }
  }

  validateForm(): boolean {
    const newErrors: MFAErrors = {};
    
    if (!this.mfaCode()) {
      newErrors.code = 'Please enter the verification code';
    } else if (this.mfaCode().length !== 6) {
      newErrors.code = 'Please enter the complete 6-digit code';
    }
    
    this.errors.set(newErrors);
    return Object.keys(newErrors).length === 0;
  }

  async handleSubmit(event: Event) {
    event.preventDefault();
    if (this.validateForm()) {
      this.isLoading.set(true);
      try {
        const identifier = this.mfaMethod() === 'email' ? this.userData.email : this.userData.phoneNumber;
        const result = await this.mfaService.verifyCode(identifier, this.mfaCode());
        
        if (result.success) {
          this.onComplete.emit({ mfaVerified: true, mfaMethod: this.mfaMethod() });
        } else {
          this.errors.set({ code: result.message });
        }
      } catch (error) {
        this.errors.set({ code: 'Failed to verify code. Please try again.' });
      } finally {
        this.isLoading.set(false);
      }
    }
  }

  handleResendCode() {
    this.sendMFACode();
  }

  handleSkipMFA() {
    this.onComplete.emit({ mfaVerified: false, mfaMethod: null });
  }

  setMFAMethod(method: 'email' | 'sms') {
    this.mfaMethod.set(method);
  }

  goBack() {
    this.isCodeSent.set(false);
  }
}

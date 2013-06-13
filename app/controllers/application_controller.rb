class ApplicationController < ActionController::Base
  protect_from_forgery

  def welcome_text(user)
    url = "http://pill-box.herokuapp.com/"
    welcome_message = "Welcome to Pillbox #{user.name}! Login now to manage your Pillbox #{url}"
    send_text(user.phone_number, welcome_message)
  end

  def send_text(number_to_send_to, message)
    twilio_sid = ENV['TWILIO_SID']
    twilio_token = ENV['TWILIO_TOKEN']
    twilio_phone_number = ENV['TWILIO_NUMBER']

    @twilio_client = Twilio::REST::Client.new twilio_sid, twilio_token

    @twilio_client.account.sms.messages.create(
      :from => "+1#{twilio_phone_number}",
      :to => number_to_send_to.to_s,
      :body => message
    )

  end

  private

  def after_sign_in_path_for(resource)
    request.env['omniauth.origin'] || "/mypillbox" || stored_location_for(resource) || root_path
  end

end

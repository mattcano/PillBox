class ApplicationController < ActionController::Base
  protect_from_forgery

  def welcome_text(user)
    user_phone = user.phone_number.gsub(/[^0-9a-z]/i, '')
    url = "http://pill-box.herokuapp.com/"
    welcome_message = "Welcome to Pillbox #{user.name}! Login now to manage your Pillbox #{url}"
    send_text(user_phone, welcome_message)
  end

  def reminder_text(user, reminder)
    user_phone = user.phone_number.gsub(/[^0-9a-z]/i, '')
    reminder_message = "Pillbox: #{user.name}, this is a friendly Pillbox reminder to #{reminder.message}, #{reminder.date.localtime.strftime("%m/%d/%y")}."
    send_text(user_phone, reminder_message)
  end

  def send_text(number_to_send_to, message)
    twilio_sid = ENV['TWILIO_SID']
    twilio_token = ENV['TWILIO_TOKEN']
    twilio_phone_number = ENV['TWILIO_NUMBER']

    @twilio_client = Twilio::REST::Client.new twilio_sid, twilio_token

    @twilio_client.account.sms.messages.create(
      :from => "+1#{twilio_phone_number}",
      :to => number_to_send_to,
      :body => message
    )

  end

  private

  def after_sign_in_path_for(resource)
    request.env['omniauth.origin'] || "/mypillbox" || stored_location_for(resource) || root_path
  end

end

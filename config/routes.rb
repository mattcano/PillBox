PillBox::Application.routes.draw do

  resources :reminders

  resources :medications

  devise_for :users, :controllers => {:registrations => "registrations"}

  root :to => 'home#index'
  get "/about" => "home#about"
  get "/credits" => "home#credits"

  get "/mypillbox" => "pillbox#mypillbox"
  get "/myreminders" => "pillbox#reminders"
  get "/meds_list" => "pillbox#meds_list"
  get "/coaches" => "pillbox#coaches"
  get "/buddies" => "pillbox#buddies"
  get "/settings" => "pillbox#settings"

  get 'user_root' => 'pillbox#mypillbox'

  post "/send_email_reminder/:id", :controller => "reminders", :action => "send_email_reminder"
  post "/send_text_reminder/:id", :controller => "reminders", :action => "send_text_reminder"
  post "/send_voice_reminder/:id", :controller => "reminders", :action => "send_voice_reminder"

  post "/reminder_call", :controller => "reminders", :action => "reminder_call"
  post "/directions", :controller => "reminders", :action => "directions"
  post "/goodbye", :controller => "reminders", :action => "goodbye"
end

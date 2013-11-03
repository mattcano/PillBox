PillBox::Application.routes.draw do

  mount RailsAdmin::Engine => '/admin', :as => 'rails_admin'

  resources :reminders

  resources :medications

  devise_for :users, :controllers => {:registrations => "registrations"}

  root :to => 'home#index'
  match "/about" => "home#about"
  match "/credits" => "home#credits"

  match "/mypillbox" => "pillbox#mypillbox"
  match "/myreminders" => "pillbox#reminders"
  match "/meds_list" => "pillbox#meds_list"
  match "/coaches" => "pillbox#coaches"
  match "/buddies" => "pillbox#buddies"
  match "/settings" => "pillbox#settings"

  match 'user_root' => 'pillbox#mypillbox'

  post "/send_email_reminder/:id", :controller => "reminders", :action => "send_email_reminder"
  post "/send_text_reminder/:id", :controller => "reminders", :action => "send_text_reminder"
  post "/send_voice_reminder/:id", :controller => "reminders", :action => "send_voice_reminder"

  post "/reminder_call/:id", :controller => "reminders", :action => "reminder_call"
  post "/directions", :controller => "reminders", :action => "directions"
  post "/goodbye", :controller => "reminders", :action => "goodbye"
end

source 'https://rubygems.org'

gem 'devise'
# gem 'rails_admin'
gem 'twilio-ruby'

gem 'rails', '3.2.13'
gem 'rake', '10.0.4'
gem 'rufus-scheduler', :require => "rufus/scheduler"

# Bundle edge Rails instead:
# gem 'rails', :git => 'git://github.com/rails/rails.git'

group :test do
  gem 'simplecov'
  gem 'rspec-rails'
  gem 'capybara'
end

group :production do
  gem "pg"
end

group :development do
  gem 'sqlite3'
end

# Gems used only for assets and not required
# in production environments by default.
group :assets do
  # gem 'sass-rails',   '~> 3.2.3'
  gem 'coffee-rails', '~> 3.2.1'
  gem 'sass-rails', '>= 3.2'

  # See https://github.com/sstephenson/execjs#readme for more supported runtimes
  # gem 'therubyracer', :platforms => :ruby

  gem 'uglifier', '>= 1.0.3'
end

# If you use Rails 3.2, make sure bootstrap-sass is moved out of the :assets group. This is because, by default in Rails 3.2, assets group gems are not required in production. However, for pre-compilation to succeed in production, bootstrap-sass gem must be required.
gem 'bootstrap-sass', '~> 3.1.1'

gem 'jquery-rails'

# To use ActiveModel has_secure_password
# gem 'bcrypt-ruby', '~> 3.0.0'

# To use Jbuilder templates for JSON
# gem 'jbuilder'

# Use unicorn as the app server
# gem 'unicorn'

# Deploy with Capistrano
# gem 'capistrano'

# To use debugger
# gem 'debugger'
